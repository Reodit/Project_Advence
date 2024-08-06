using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StageManager : Singleton<StageManager>
{
    // Stage Logic 
    public int currentMonsterCount;

    protected override void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        LoadUserUnLockStage();
        
    }
    
    private void LoadUserUnLockStage()
    {
        // Get Player Data and Get Current Stages;
        GetPlayerData();
        
        // 
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
}
