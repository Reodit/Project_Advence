using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stage
{
    //public int 
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform characterSpawnPoint;
    public MoveArea MoveArea;
    
    public IngameUI IngameUI;
    public PlayerMove PlayerMove { get; private set; }
    public MonsterSpawner MonsterSpawner;
    public int phaseCount;
    public int currentStage;
    
    public GameObject testMonster;
    public GameObject testBossMonster;
    //public int Stage
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        isGamePause = false;
        GameDataLoad();
        PlayerInstantiate(1);
        
        IngameUI.Instance.Init();
        currentStage = 1;
        MonsterSpawner.Init();
        phaseCount = MonsterSpawner.Phases.Count;
        
    }

    private void GameDataLoad()
    {
        Datas.GameData.LoadCharacterDataToGameData("CharacterTable");
        Datas.GameData.LoadCharacterLevelDataToGameData("CharacterLevelTable");
        Datas.GameData.LoadSkillDataToGameData("SkillTable");
        Datas.GameData.LoadSelectStatDataToGameData("SelectStatTable");
        Datas.GameData.LoadSkillEnchantDataToGameData("SkillEnchantTable");
        Datas.GameData.LoadStatLevelDataToGameData("StatLevelTable");
        Datas.GameData.LoadPatternDataToGameData("PatternTable");
        Datas.GameData.LoadPhaseDataToGameData("PhaseTable");
    }
    

    // 게임 시작 시 플레이어 스폰
    private void PlayerInstantiate(int id)
    {
        // 캐릭터 부모 하위에 인스턴싱
        var player = Instantiate(Resources.Load<GameObject>(Datas.GameData.DTCharacterData[id].prefabPath), characterSpawnPoint);
        
        // 컴포넌트 찾아서 넣어주기
        PlayerMove = player.GetComponent<PlayerMove>();
        PlayerMove.characterData = Datas.GameData.DTCharacterData[id];
        PlayerMove.Init();
    }
    public void Update()
    {
        
    }

    public bool isGamePause;
    public void PauseGame()
    {
        Time.timeScale = 0; // 게임 일시정지
        isGamePause = true;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1; // 게임 재개
        isGamePause = false;
    }

    [ContextMenu("test")]
    public void BgTest()
    {
        foreach (var e in ImageScrolling.Instace.scrollingImages[GameManager.Instance.currentStage - 1])
        {
            e.gameObject.SetActive(false);   
        }

        GameManager.Instance.currentStage++;
    }
}
