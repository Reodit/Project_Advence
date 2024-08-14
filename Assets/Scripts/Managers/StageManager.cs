using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// + Monster Spawn
public class StageManager : Singleton<StageManager>
{
    // Stage Logic 
    public int currentMonsterCount;

    // Phases data
    public Dictionary<int, List<Phase>> StageDictionary { get; private set; }
    private static float _currentSpace;
    private GameObject _monsterSpawnObject;
    private List<Transform> _spawnPoints;
    public float outOfScreenXPos = -20f;
    public Phase currentPhase;
    [SerializeField] private float targetXPos;
    [SerializeField] private float phaseTime = 360f; 
    public int totalMonsterCount;
    public int phaseCountInCurrentStage;
    
    
    public void Initialize()
    {
        // TODO 유저의 Unlock정보를 받아오기
        LoadUserUnLockStage();

        // TODO Monster Pool 생성하기
        var monsterData = Datas.GameData.DTMonsterData;
        var bulletData = Datas.GameData.DTSkillData;

        List<string> monsterPrefabs =
            monsterData.Select(e => e.Value.PrefabPath).ToList();
        List<string> bulletPrefabs =
            bulletData.Select(e => e.Value.prefabPath).ToList();

        // Reset-Create Pool
        ObjectPoolManager.instance.ResetPools();
        ObjectPoolManager.instance.CreatePool("Monster", monsterPrefabs, 64, 1024);
        ObjectPoolManager.instance.CreatePool("Bullet", bulletPrefabs, 64, 512);
        
        if (StageDictionary == null)
        {
            StageDictionary = new Dictionary<int, List<Phase>>();
            var phases = Datas.GameData.DTPhaseData.Values.Select(e => new Phase(e, phaseTime)).ToList();

            foreach (var phase in phases)
            {
                if (!StageDictionary.ContainsKey(phase.phaseData.stage))
                {
                    StageDictionary[phase.phaseData.stage] = new List<Phase>();
                }

                StageDictionary[phase.phaseData.stage].Add(phase);
            }
        }

        if (currentPhase == null)
        {
            LoadStage(1);
            _monsterSpawnObject = GameObject.Find("MonsterSpawnPoint");
            _spawnPoints = new List<Transform>();
            for (int i = 0; i < _monsterSpawnObject.transform.childCount; i++)
            {
                _spawnPoints.Add(_monsterSpawnObject.transform.GetChild(i));
            }
        }
        _currentSpace = 0;
    }

    public void CleanupStage()
    {
        ObjectPoolManager.instance.ResetPools();
    }
    
    private void LoadUserUnLockStage()
    {
        // Get Player Data and Get Current Stages;
        GetPlayerData();
        
        // 
    }

    public void LoadStage(int stageNumber)
    {
        // TODO 유저가 선택한 페이즈의 1번으로 시작해야함. 그 전까지는 없어야함
        currentPhase = StageDictionary[stageNumber][0];
        targetXPos = this.transform.position.x + currentPhase!.phaseData.firstPrintMonster;
        phaseCountInCurrentStage = StageDictionary[stageNumber].Count;
    }
    
    private void GetPlayerData()
    {
        
    }
    
    // Stage start ==> normal ==> boss

    private void MoveNextStage()
    {
        
    }

    
    // GetStageDatas
    private void GetCurrentStage()
    {
        
    }
    
    private void GetCurrentPhase()
    {
        
    }
    
    PatternTable SelectPattern(Phase phase)
    {
        if (currentMonsterCount <= phase.phaseData.targetMonsterValue * 
            (1 - phase.remainTime / phase.phaseTime) * phase.phaseData.phaseValue2)
        {
            if (currentMonsterCount <= phase.phaseData.targetMonsterValue *
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


    /*public void MoveNextPhase()
    {
        isbossing = false;
        
        if (phases.Count > 1)
        {
            phases.Remove(currentPhase);
            currentPhase = phases[0];
            GameManager.instance.currentStage = currentPhase.phaseData.stage;
            ImageScrolling.Instance.scrollSpeed = currentPhase.phaseData.scrollSpeed;
            GameManager.instance.phaseCountInCurrentStage = phases.Count(phase =>
                phase.phaseData.stage == GameManager.instance.currentStage);
            GameManager.instance.currentPhaseNumber = phases[GameManager.instance.currentStage].phaseData.phaseNumber;
        }

        else
        {
            // 마지막 스테이지에 대한 처리
            // TODO 다음 스테이지 이동
            Debug.Log("Stage Clear");
            GameManager.instance.PauseGame();
        }
    }*/
    
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
                    var monsterPrefab = Resources.Load<GameObject>(Datas.GameData.DTMonsterData[monsterID].PrefabPath);
                    var monster = ObjectPoolManager.instance.SpawnFromPool("Monster", 
                            Datas.GameData.DTMonsterData[monsterID].PrefabPath, transform.position, Quaternion.identity, monsterPrefab.transform.localScale, _spawnPoints[i - 1]);
                    
                    monster.transform.position += new Vector3(_currentSpace, 0f, 0f);
                    totalMonsterCount++;
                }
            }
        }
        
        _currentSpace += pattern.patternInterval;
    }

    private bool isbossing;
    
    IEnumerator MoveBossPhase()
     {
         //isbossing = true;
         //var bossMonster = Instantiate(GameManager.instance.bossPrefab);
         //bossMonster.transform.position = new Vector3(7f, 2f, 0f);

         //var boss = bossMonster.GetComponent<Monster>() as S1P1BossMonster;
         //yield return new WaitUntil(() => boss.CurrentHp <= 0);
         
         //MoveNextPhase();
        
        yield return null;
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "IngameScene")
        {
            return;
        }
        
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
            Vector3 currentPosition = _monsterSpawnObject.transform.position;
            Vector3 newPosition = new Vector3(currentPosition.x - currentPhase.phaseData.scrollSpeed * Time.deltaTime, currentPosition.y, 0f);
            _monsterSpawnObject.transform.position = newPosition;    

            if (_monsterSpawnObject.transform.position.x <= targetXPos)
            {
                var pattern = SelectPattern(currentPhase);
                targetXPos -= pattern.patternInterval;
                MonsterSpawn(pattern);
            }
        }
    }
}
