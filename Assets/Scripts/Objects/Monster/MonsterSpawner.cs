using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;
 

[Serializable]
public class Phase
{
    public PhaseTable phaseData;
    public List<PatternTable> patternList;
    public float remainTime;
    public float phaseTime;

    public Phase(PhaseTable phaseData, List<PatternTable> patternList, float phaseTime)
    {
        this.phaseData = phaseData;
        this.patternList = patternList;
        this.phaseTime = phaseTime;
        remainTime = phaseTime;
    }
    
    public Phase(PhaseTable phaseData, float phaseTime)
    {
        this.phaseData = phaseData;
        this.patternList = AddPattern();
        this.phaseTime = phaseTime;

        remainTime = phaseTime;
    }

    private List<PatternTable> AddPattern()
    {
        var patterns = new List<PatternTable>();
        
        foreach (var e in Datas.GameData.DTPatternData)
        {
            patterns.Add(e.Value);    
        }

        return patterns;
    }
}

public class MonsterSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public float outOfScreenXPos = -20f;
    public float scrollSpeed;
    public float initialXPos;
    public Phase currentPhase;
    [SerializeField] private float targetXPos;

    [SerializeField] private float phaseTime = 360f; 
    // TODO 풀링 대상
    public int totalMonsterCount;
    public static float currentSpace;

    public List<Phase> phases;

    public void Init()
    {
        phases = new List<Phase>();
        foreach (var e in Datas.GameData.DTPhaseData.Values)
        {
            phases.Add(new Phase(e, phaseTime));
        }
        
        currentSpace = 0;
        currentPhase = phases.FirstOrDefault();
        targetXPos = this.transform.position.x + currentPhase.phaseData.firstPrintMonster;
        GameManager.Instance.currentStage = currentPhase.phaseData.stage;
        ImageScrolling.Instace.scrollSpeed = currentPhase.phaseData.scrollSpeed;
        GameManager.Instance.phaseCountInCurrentStage = phases.Count(phase =>
            phase.phaseData.stage == GameManager.Instance.currentStage);
        GameManager.Instance.currentPhaseNumber = phases[GameManager.Instance.currentStage].phaseData.phaseNumber;
    }

    public void MoveNextPhase()
    {
        isbossing = false;
        
        if (phases.Count > 1)
        {
            phases.Remove(currentPhase);
            currentPhase = phases[0];
            GameManager.Instance.currentStage = currentPhase.phaseData.stage;
            ImageScrolling.Instace.scrollSpeed = currentPhase.phaseData.scrollSpeed;
            GameManager.Instance.phaseCountInCurrentStage = phases.Count(phase =>
                phase.phaseData.stage == GameManager.Instance.currentStage);
            GameManager.Instance.currentPhaseNumber = phases[GameManager.Instance.currentStage].phaseData.phaseNumber;
        }

        else
        {
            // 마지막 스테이지에 대한 처리
            // TODO 다음 스테이지 이동
            Debug.Log("Stage Clear");
            GameManager.Instance.PauseGame();
        }
    }
    
    void MonsterSpawn(PatternTable pattern)
    {
        for (int i = 1; i <= 5; i++)
        {
            FieldInfo fieldInfo = typeof(PatternTable).GetField($"vertical{i}", BindingFlags.Public | BindingFlags.Instance);
    
            if (fieldInfo != null)
            {
                var value = fieldInfo.GetValue(pattern);

                if (value != null && !isbossing)
                {
                    int monsterID = (int)value;
                    if (monsterID == 0)
                    {
                        continue;
                    }
                    
                    var monster = ObjectPooler.Instance.Monster.GetFromPool(
                        Resources.Load<Monster>(Datas.GameData.DTMonsterData[monsterID].PrefabPath), 
                        transform.position, Quaternion.identity, spawnPoints[i - 1]);
                    monster.transform.position += new Vector3(currentSpace, 0f, 0f);
                    totalMonsterCount++;
                }
            }
        }
        
        currentSpace += pattern.patternInterval;
    }

    private bool isbossing;
    
    IEnumerator MoveBossPhase()
    {
        isbossing = true;
        var bossMonster = Instantiate(GameManager.Instance.bossPrefab);
        bossMonster.transform.position = new Vector3(7f, 2f, 0f);

        var boss = bossMonster.GetComponent<Monster>() as S1P1BossMonster;
        yield return new WaitUntil(() => boss.CurrentHp <= 0);
        
        MoveNextPhase();
    }
    
    void Update()
    {
        if (isbossing)
        {
            currentPhase.remainTime = 0f;
        }

        else
        {
            currentPhase.remainTime -= Time.deltaTime;
        }
        
        if (currentPhase.remainTime <= 0 && !isbossing)
        {
            StartCoroutine(MoveBossPhase());            
            totalMonsterCount = 0;
        }

        else
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x - scrollSpeed * Time.deltaTime, currentPosition.y, 0f);
            transform.position = newPosition;    

            if (transform.position.x <= targetXPos)
            {
                var pattern = SelectPattern(currentPhase);
                targetXPos -= pattern.patternInterval;
                MonsterSpawn(pattern);
            }
        }
    }

    PatternTable SelectPattern(Phase phase)
    {
        if (totalMonsterCount <= phase.phaseData.targetMonsterValue * 
            (1 - phase.remainTime / phase.phaseTime) * phase.phaseData.phaseValue2)
        {
            if (totalMonsterCount <= phase.phaseData.targetMonsterValue *
                (1 - phase.remainTime / phase.phaseTime) * phase.phaseData.phaseValue1)
            {
                var filteredPatterns = phase.patternList.Where(pattern => 
                    pattern.monsterCnt >= 3).ToList();
                
                if (filteredPatterns.Count > 0)
                {
                    int randomIndex = Random.Range(0, filteredPatterns.Count);
                    return filteredPatterns[randomIndex];
                }

                return null;
            }

            else
            {
                var filteredPatterns = phase.patternList.Where(pattern => pattern.monsterCnt < 3).ToList();
                
                if (filteredPatterns.Count > 0)
                {
                    int randomIndex = Random.Range(0, filteredPatterns.Count);
                    return filteredPatterns[randomIndex];
                }

                return null;
            }
        }

        else
        {
            var filteredPatterns = phase.patternList.Where(pattern => pattern.monsterCnt == 0).ToList();

            if (filteredPatterns.Count > 0)
            {
                int randomIndex = Random.Range(0, filteredPatterns.Count);
                return filteredPatterns[randomIndex];
            }

            return null;
        }
    }
    
}
