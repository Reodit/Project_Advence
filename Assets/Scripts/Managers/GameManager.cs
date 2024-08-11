using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 모든 매니저 코드 초기화 & 일반적인 기능 담담
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public Camera mainCamera;
    public Transform characterSpawnPoint;
    // public MoveArea MoveArea;
    
    public PlayerMove PlayerMove { get; private set; }
    public MonsterSpawner MonsterSpawner { get; private set; }
        
    [Header("Stage & Phase")]


    // TODO : move to input manager
    [Header("Input")] 
    public FixedJoystick fixedJoystick;
    public bool IsGamePaused { get; private set; }

    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Preload -- Preload는 최초 한번 이외에는 갈일 없음
        mainCamera = Camera.main;
        GameDataLoad();
        
        // Game delta time
        IsGamePaused = false;
    }

    private void GameDataLoad()
    {
        Datas.PlayerData.LoadCharacterStatData();
        Datas.GameData.LoadCharacterDataToGameData("CharacterTable");
        Datas.GameData.LoadCharacterLevelDataToGameData("CharacterLevelTable");
        Datas.GameData.LoadSkillDataToGameData("SkillTable");
        Datas.GameData.LoadSelectStatDataToGameData("SelectStatTable");
        Datas.GameData.LoadSkillEnchantDataToGameData("SkillEnchantTable");
        Datas.GameData.LoadStatLevelDataToGameData("StatLevelTable");
        Datas.GameData.LoadPatternDataToGameData("PatternTable");
        Datas.GameData.LoadPhaseDataToGameData("PhaseTable");
        Datas.GameData.LoadMonsterDataToGameData("MonsterTable");
        Datas.GameData.LoadFamiliarDataToGameData("FamiliarData");
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PreLoadScene")
        {
            // Move Lobby Scene (OutGame Scene)
            SceneManager.LoadScene("OutgameScene");
        }
        
        else if (scene.name == "OutgameScene")
        {
            StageManager.instance.CleanupStage();
        }
        
        else if (scene.name == "IngameScene")
        {
            // In-game Initialize
            fixedJoystick = FindObjectOfType<FixedJoystick>();
            PlayerInstantiate(1);

            StageManager.instance.Initialize();
        }

        else
        {
            Debug.LogError($"This scene is not registered. SceneName = {SceneManager.GetActiveScene().name}");
        }
    }
    
    // TODO Move Stage Manager 
    public void PlayerInstantiate(int id)
    {
        var player = Instantiate(Resources.Load<GameObject>(Datas.GameData.DTCharacterData[id].prefabPath), characterSpawnPoint);
        
        PlayerMove = player.GetComponent<PlayerMove>();
        PlayerMove.characterData = Datas.GameData.DTCharacterData[id];
        PlayerMove.Init();
    }

    public void PauseGame()
    {
        if (!IsGamePaused)
        {
            Time.timeScale = 0f;
            IsGamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if (IsGamePaused)
        {
            Time.timeScale = 1f;
            IsGamePaused = false;
        }
    }
    
    // TODO Move Stage Manager
    public void StageInitialize()
    {
        
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
