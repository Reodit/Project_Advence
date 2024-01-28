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
    public MoveArea MoveArea; // 할당
    
    public IngameUI IngameUI;
    public PlayerMove PlayerMove { get; private set; }
    public MonsterSpawner MonsterSpawner;
    public int phaseCount;
    public int currentPhase;

    public GameObject testMonster;
    public GameObject testBossMonster;
    //public int Stage
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        GameDataLoad();
        PlayerInstantiate(1);
        
        IngameUI.Instance.Init();
        currentPhase = 0;
        MonsterSpawner.Init();
        phaseCount = MonsterSpawner.Phases.Count;
        
    }

    private void GameDataLoad()
    {
        Datas.GameData.LoadCharacterDataToGameData("CharacterTable");
        Datas.GameData.LoadCharacterLevelDataToGameData("CharacterLevelTable");
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
    
    public void PauseGame()
    {
        Time.timeScale = 0; // 게임 일시정지
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1; // 게임 재개
    }

    
}
