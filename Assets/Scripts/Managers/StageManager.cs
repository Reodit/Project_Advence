using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor.iOS;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    public int currentMonsterCount;

    public Dictionary<int, List<Phase>> StageDictionary { get; private set; }
    private static float _currentSpace;
    private GameObject _monsterSpawnObject;
    private List<Transform> _spawnPoints;
    public float outOfScreenXPos = -20f;
    public Phase currentPhase;
    [SerializeField] private float targetXPos;
    [SerializeField] private float phaseTime = 30; 
    public int phaseCountInCurrentStage;
    
    // Stage에서 저장해야 하는 변수들
    // ex. 점수/획득 골드/총 몬스터 수?
    public int totalMonsterCount;
    public int goldCurrentStage { get; private set; }
    public int scoreCurrentStage { get; private set; }

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
        ObjectPoolManager.instance.CreatePool("Monster", monsterPrefabs, 100, 300);
        ObjectPoolManager.instance.CreatePool("Bullet", bulletPrefabs, 100, 300);
        
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
    }

    public void OnMonsterDie(Monster monster)
    {
        goldCurrentStage += monster.monsterData.Gold;
        scoreCurrentStage += monster.monsterData.Score;
        GameManager.instance.PlayerMove.currentExp += monster.monsterData.EXP;
    }
    
    public void LoadStage(int stageNumber)
    {
        // TODO 유저가 선택한 페이즈의 1번으로 시작해야함. 그 전까지는 없어야함
        currentPhase = StageDictionary[stageNumber][0];
        targetXPos = this.transform.position.x + currentPhase!.phaseData.firstPrintMonster;
        phaseCountInCurrentStage = StageDictionary[stageNumber].Count;
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


    public IEnumerator MoveNextPhase()
    {
        _isBossPhase = false;
        
        if (currentPhase.phaseData.phaseNumber < 
            StageDictionary[currentPhase.phaseData.stage].Max(phase => phase.phaseData.phaseNumber))
        {
            currentPhase = StageDictionary[currentPhase.phaseData.stage][currentPhase.phaseData.phaseNumber + 1];
            // image scrolling에서 업데이트 한번 쳐주기
            // ImageScrolling.Instance.scrollSpeed = currentPhase.phaseData.scrollSpeed;
            // GameManager.instance.phaseCountInCurrentStage = phases.Count(phase =>
            //     phase.phaseData.stage == GameManager.instance.currentStage);
            // GameManager.instance.currentPhaseNumber = phases[GameManager.instance.currentStage].phaseData.phaseNumber;
        }

        else
        {
            // TODO 다음 스테이지 이동
            isStopSpawn = true;
            PopupManager.instance.InstantiatePopUp("UIPrefabs/UI_Result_Clear");
            yield return null;
        }
    }

    public bool isStopSpawn;
    
    void MonsterSpawn(PatternTable pattern)
    {
        for (int i = 1; i <= 5; i++)
        {
            FieldInfo fieldInfo = typeof(PatternTable).GetField($"vertical{i}", BindingFlags.Public | BindingFlags.Instance);
    
            if (fieldInfo != null)
            {
                var value = fieldInfo.GetValue(pattern);

                if (value != null && !_isBossPhase)
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

    private bool _isBossPhase;
    
    // TODO 중간보스 일 경우 처리
    IEnumerator MoveBossPhase(int bossNumber)
     {
         _isBossPhase = true;
         var stage = currentPhase.phaseData.stage;
         var bossMonster = Instantiate(GameManager.instance.BossPrefabs[bossNumber]);
         bossMonster.transform.position = new Vector3(7f, 2f, 0f);

         var boss = bossMonster.GetComponent<Monster>();
         // 비동기로 동작하면서 Start가 더 늦게 불리는 부분 하드코딩
         yield return new WaitForSeconds(0.1f);
         yield return new WaitUntil(() => boss.CurrentHp <= 0);
         
         yield return StartCoroutine(MoveNextPhase());
        
        yield return null;
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "IngameScene")
        {
            return;
        }

        if (isStopSpawn)
        {
            return;
        }
        
        if (_isBossPhase)
        {
            currentPhase.remainTime = 0f;
        }

        else
        {
            currentPhase.remainTime -= Time.deltaTime;
        }
        
        if (currentPhase.remainTime <= 0 && !_isBossPhase)
        {
            StartCoroutine(MoveBossPhase(currentPhase.phaseData.index-50));            
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
