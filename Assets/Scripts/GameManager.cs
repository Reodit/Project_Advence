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

    public IngameUI IngameUI;
    public PlayerMove PlayerMove;
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
        PlayerMove.Init();
        IngameUI.Instance.Init();
        currentPhase = 0;
        MonsterSpawner.Init();
        phaseCount = MonsterSpawner.Phases.Count;
        
    }

    public void Update()
    {
        
    }
    
    
    
}
