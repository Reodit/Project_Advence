using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


[Serializable]
public class Phase
{
    public PhaseTable phaseData;
    public List<PatternTable> patternList;
    public float remainTime;
    public float phaseTime;
    /// <summary>
    /// 커스텀 패턴 추가가 필요할 때
    /// </summary>
    /// <param name="phaseData"></param>
    /// <param name="patternList"></param>
    public Phase(PhaseTable phaseData, List<PatternTable> patternList, float phaseTime)
    {
        this.phaseData = phaseData;
        this.patternList = patternList;
        this.phaseTime = phaseTime;
        remainTime = phaseTime;
    }

    /// <summary>
    /// 데이터 테이블에 있는 패턴을 사용
    /// </summary>
    /// <param name="phaseData"></param>
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
    [FormerlySerializedAs("SpawnPoints")] public List<Transform> spawnPoints;
    // TODO 이거 정리
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
            // 다른거 개발될때 까지 일단 멈춥시다. (태훈)
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
                    
                    var monster = Instantiate(
                        Resources.Load<GameObject>(Datas.GameData.DTMonsterData[monsterID].PrefabPath), 
                        spawnPoints[i-1]);
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
        // 보스 인스턴싱
        var bossMonster = Instantiate(GameManager.Instance.testBossMonster);
        bossMonster.transform.position = new Vector3(7f, 2f, 0f);

        var boss = bossMonster.GetComponent<Monster>();

        // 보스 처리
        yield return new WaitUntil(() => boss.CurrentHp <= 0);
        
        // 다음 페이즈 이동
        MoveNextPhase();
    }
    
    // Update is called once per frame
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
                
                if (filteredPatterns.Count > 0) // 필터링된 리스트에 요소가 존재하는 경우
                {
                    int randomIndex = Random.Range(0, filteredPatterns.Count); // 랜덤 인덱스 생성
                    return filteredPatterns[randomIndex]; // 랜덤 요소 선택
                }

                return null;
            }

            else
            {
                var filteredPatterns = phase.patternList.Where(pattern => pattern.monsterCnt < 3).ToList();
                
                if (filteredPatterns.Count > 0) // 필터링된 리스트에 요소가 존재하는 경우
                {
                    int randomIndex = Random.Range(0, filteredPatterns.Count); // 랜덤 인덱스 생성
                    return filteredPatterns[randomIndex]; // 랜덤 요소 선택
                }

                return null;
            }
        }

        else
        {
            var filteredPatterns = phase.patternList.Where(pattern => pattern.monsterCnt == 0).ToList();

            if (filteredPatterns.Count > 0) // 필터링된 리스트에 요소가 존재하는 경우
            {
                int randomIndex = Random.Range(0, filteredPatterns.Count); // 랜덤 인덱스 생성
                return filteredPatterns[randomIndex]; // 랜덤 요소 선택
            }

            return null;
        }
    }
    
}
