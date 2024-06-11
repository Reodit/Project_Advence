using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    // Stage Logic 


    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        LoadStage();
        
    }
    
    private void LoadStage()
    {
        // Get Player Data and Get Current Stages;
        GetPlayerData();
        
        // 
    }

    private void GetPlayerData()
    {
        
    }
}
