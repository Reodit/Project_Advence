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
    public float remainTime;
    public float PhaseTime;
    public float firstPrintMonster;
    public int targetMonsterValue;
    public List<Pattern> Patterns;
    public float phaseValue1;
    public float phaseValue2;
    public Phase(float phaseTime, float firstPrintMonster, int targetMonsterValue, float phaseValue1, float phaseValue2)
    {
        this.remainTime = phaseTime;
        this.PhaseTime = phaseTime;
        this.firstPrintMonster = firstPrintMonster;
        this.targetMonsterValue = targetMonsterValue;
        this.phaseValue1 = phaseValue1;
        this.phaseValue2 = phaseValue2;

        AddPattern();
    }

    public void AddPattern()
    {
        this.Patterns = new List<Pattern>();

        var monster = GameManager.Instance.testMonster;

        Patterns.Add(new Pattern(10f, monster, null, null, null, null, 0));
        Patterns.Add(new Pattern(5f, monster, monster, null, null, null, 0));
        Patterns.Add(new Pattern(20f, monster, monster, monster, null, null, 1));
        Patterns.Add(new Pattern(15f, monster, monster, monster, monster, null, 1));
        Patterns.Add(new Pattern(10f, monster, monster, monster, monster, monster, 2));
    }
}

[Serializable]
public class Pattern
{
    public float monsterSpace;

    public GameObject vertical1;
    public GameObject vertical2;
    public GameObject vertical3;
    public GameObject vertical4;   
    public GameObject vertical5;

    public int logic;

    public Pattern(float monsterSpace, GameObject vertical1, 
        GameObject vertical2, GameObject vertical3, GameObject vertical4, GameObject vertical5,
        int logic)
    {
        this.monsterSpace = monsterSpace;
        this.vertical1 = vertical1;
        this.vertical2 = vertical2;
        this.vertical3 = vertical3;
        this.vertical4 = vertical4;
        this.vertical5 = vertical5;
        this.logic = logic;
    }
}

public class MonsterSpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public float outOfScreenXPos = -20f;
    public List<GameObject> MonsterList;
    public float scrollSpeed;
    public float initialXPos;
    public Phase currentPhase;
    [SerializeField] private float targetXPos;

    public static int totalMonsterCount;
    public static float currentSpace;
    public int trackingCount;
    public List<Phase> Phases = new List<Phase>();
    // 1 Phase = 5분 = 360f 스크롤 시간과 관계 없
    // 

    private void Awake()
    {
    }

    void Start()
    {
        
    }

    public void Init()
    {
        targetXPos = this.transform.position.x + initialXPos;
        currentSpace = 0;

        if (Phases != null && Phases.Count <= 0)
        {
            Phases.Add(new Phase(20f, 10f, 50, 0.8f, 1.2f));
            Phases.Add(new Phase(50f, 10f, 150,  0.8f, 1.2f));
            Phases.Add(new Phase(360f, 10f, 300,  0.8f, 1.2f));
        }

        currentPhase = Phases[0];
    }

    public void MoveNextPhase()
    {
        isbossing = false;
        
        if (Phases.Count > 1)
        {
            Phases.Remove(currentPhase);
            currentPhase = Phases[0];
        }

        else
        {
            // 마지막 페이즈에 대한 처리
            // 다음 스테이지 이동
            Debug.Log("Stage Clear");
            foreach (var e in ImageScrolling.Instace.scrollingImages[GameManager.Instance.currentStage - 1])
            {
                e.gameObject.SetActive(false);   
            }

            GameManager.Instance.currentStage++;
        }
    }
    
    void MonsterSpawn(Pattern pattern)
    {
        for (int i = 1; i <= 5; i++)
        {
            FieldInfo fieldInfo = typeof(Pattern).GetField($"vertical{i}", BindingFlags.Public | BindingFlags.Instance);
    
            if (fieldInfo != null)
            {
                var value = fieldInfo.GetValue(pattern);

                if (value != null && !isbossing)
                {
                    var monster = Instantiate(value as GameObject, SpawnPoints[i-1]);
                    monster.transform.position += new Vector3(currentSpace, 0f, 0f);
                    totalMonsterCount++;
                }
            }
        }
        
        currentSpace += pattern.monsterSpace;
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
        yield return new WaitUntil(() => boss.CurrentHp() <= 0);
        
        // 다음 페이즈 이동
        MoveNextPhase();
    }
    
    // Update is called once per frame
    void Update()
    {
        trackingCount = totalMonsterCount;
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
                targetXPos -= pattern.monsterSpace;
                MonsterSpawn(pattern);
            }
        }
    }
    
    Pattern SelectPattern(Phase phase)
    {
        if (totalMonsterCount <= phase.targetMonsterValue * 
            phase.remainTime / phase.PhaseTime * phase.phaseValue1)
        {
            if (totalMonsterCount <= phase.targetMonsterValue *
                phase.remainTime / phase.PhaseTime * phase.phaseValue2)
            {
                var filteredPatterns = phase.Patterns.Where(pattern => pattern.logic == 2).ToList();
                
                if (filteredPatterns.Count > 0) // 필터링된 리스트에 요소가 존재하는 경우
                {
                    int randomIndex = Random.Range(0, filteredPatterns.Count); // 랜덤 인덱스 생성
                    return filteredPatterns[randomIndex]; // 랜덤 요소 선택
                }

                return null;
            }

            else
            {
                var filteredPatterns = phase.Patterns.Where(pattern => pattern.logic == 1).ToList();
                
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
            var filteredPatterns = phase.Patterns.Where(pattern => pattern.logic == 0).ToList();

            if (filteredPatterns.Count > 0) // 필터링된 리스트에 요소가 존재하는 경우
            {
                int randomIndex = Random.Range(0, filteredPatterns.Count); // 랜덤 인덱스 생성
                return filteredPatterns[randomIndex]; // 랜덤 요소 선택
            }

            return null;
        }
    }
    
}
