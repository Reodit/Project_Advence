using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public Phase

public class MonsterSpawner : MonoBehaviour
{
    

    public List<Transform> SpawnPoints;
    public float outOfScreenXPos = -20f;
    public List<GameObject> MonsterList;
    public float scrollSpeed;
    public float initialXPos;
    public PhaseTable currentPhase;
    [SerializeField] private float targetXPos;

    public static int totalMonsterCount;
    public static float currentSpace;
    public int trackingCount;
    public List<PhaseTable> Phases = new List<PhaseTable>();
    // 1 Phase = 5분 = 360f 스크롤 시간과 관계 없
    // 

    private void Awake()
    {
    }

    public void AddPattern()
    {
        //this.Patterns = Datas.GameData.DTPatternData.Values.ToList();
    }

    void Start()
    {
        
    }

    public void Init()
    {
        targetXPos = this.transform.position.x + initialXPos;
        currentSpace = 0;

        currentPhase = Datas.GameData.DTPhaseData.Values.FirstOrDefault();
    }

    public void MoveNextPhase()
    {
        Debug.Log("");
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
                    var monster = Instantiate(value as GameObject, SpawnPoints[i-1]);
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
                targetXPos -= pattern.patternInterval;
                MonsterSpawn(pattern);
            }
        }
    }

    PatternTable SelectPattern(PhaseTable phase)
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
