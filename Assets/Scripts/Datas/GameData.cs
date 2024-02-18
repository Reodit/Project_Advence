
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Datas
{
    public static class GameData
    {
        public static Dictionary<int, CharacterTable> DTCharacterData = new Dictionary<int, CharacterTable>();

        public static void LoadCharacterDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var characterTableData = Resources.Load<ScriptableObject>(path) as CharacterTableScriptableObject;

            // DTCharacterData 딕셔너리에 데이터 저장
            if (characterTableData != null)
            {
                foreach (var e in characterTableData.characterTableList)
                {
                    DTCharacterData.Add(e.id, e);
                }
            }
            
            else
            {
                Debug.LogError("Failed to load CharacterTableData from ScriptableObject.");
            }
        }
        
        public static Dictionary<int, CharacterLevelTable> DTCharacterLevelData = new Dictionary<int, CharacterLevelTable>();

        public static void LoadCharacterLevelDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var characterLevelTableData = Resources.Load<ScriptableObject>(path) as CharacterLevelTableScriptableObject;

            if (characterLevelTableData != null)
            {
                foreach (var e in characterLevelTableData.characterLevelTableList)
                {
                    
                    DTCharacterLevelData.Add(e.level, e);
                }
            }
            
            else
            {
                Debug.LogError("Failed to load CharacterTableData from ScriptableObject.");
            }
        }
        
        public static Dictionary<int, StatLevelTable> DTStatLevelData = new Dictionary<int, StatLevelTable>();
        public static Dictionary<int, SelectStatTable> DTSelectStatData = new Dictionary<int, SelectStatTable>();
        public static Dictionary<int, SkillTable> DTSkillData = new Dictionary<int, SkillTable>();
        public static Dictionary<int, SkillEnchantTable> DTSkillEnchantData = new Dictionary<int, SkillEnchantTable>();
        
        public static void LoadStatLevelDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var statLevelTableData = Resources.Load<ScriptableObject>(path) as StatLevelTableScriptableObject;

            if (statLevelTableData != null)
            {
                foreach (var e in statLevelTableData.characterLevelTableList)
                {
                    
                    DTStatLevelData.Add(e.index, e);
                }
            }
            
            else
            {
                Debug.LogError("Failed to load statLevelTableData from ScriptableObject.");
            }
        }
        
        public static void LoadSelectStatDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var selectStatTableData = Resources.Load<ScriptableObject>(path) as SelectStatTableScriptableObject;

            if (selectStatTableData != null)
            {
                foreach (var e in selectStatTableData.SelectStatTableList)
                {
                    
                    DTSelectStatData.Add(e.index, e);
                }
            }
            
            else
            {
                Debug.LogError("Failed to load selectStatTableData from ScriptableObject.");
            }
        }
        
        public static void LoadSkillDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var skillData = Resources.Load<ScriptableObject>(path) as SkillTableScriptableObject;

            if (skillData != null)
            {
                foreach (var e in skillData.SkillTablesList)
                {
                    
                    DTSkillData.Add(e.index, e);
                }
            }
            
            else
            {
                Debug.LogError("Failed to load skillData from ScriptableObject.");
            }
        }
        
        public static void LoadSkillEnchantDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var skillEnchantTableData = Resources.Load<ScriptableObject>(path) as SkillEnchantTableScriptableObject;

            if (skillEnchantTableData != null)
            {
                foreach (var e in skillEnchantTableData.SkillEnchantTables)
                {
                    
                    DTSkillEnchantData.Add(e.index, e);
                }
            }
            
            else
            {
                Debug.LogError("Failed to load skillEnchantTableData from ScriptableObject.");
            }
        }

        public static Dictionary<int, PatternTable> DTPatternData = new Dictionary<int, PatternTable>();
        public static Dictionary<int, PhaseTable> DTPhaseData = new Dictionary<int, PhaseTable>();

        public static void LoadPatternDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var patternData = Resources.Load<ScriptableObject>(path) as PatternTableScriptableObject;

            if (patternData != null)
            {
                foreach (var e in patternData.Patterns)
                {

                    DTPatternData.Add(e.index, e);
                    DTPatternData.OrderBy(kvp => kvp.Key);
                }
            }

            else
            {
                Debug.LogError("Failed to load PatternData from ScriptableObject.");
            }
        }

        public static void LoadPhaseDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var phaseData = Resources.Load<ScriptableObject>(path) as PhaseTableScriptableObject;

            if (phaseData != null)
            {
                foreach (var e in phaseData.PhaseTables)
                {

                    DTPhaseData.Add(e.index, e);
                    DTPhaseData.OrderBy(kvp => kvp.Key);
                }
            }

            else
            {
                Debug.LogError("Failed to load PhaseData from ScriptableObject.");
            }
        }

        public static Dictionary<int, MonsterTable> DTMonsterData = new Dictionary<int, MonsterTable>();

        public static void LoadMonsterDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var monsterData = Resources.Load<ScriptableObject>(path) as MonsterTableScriptableObject;

            if (monsterData != null)
            {
                foreach (var e in monsterData.MonsterTables)
                {

                    DTMonsterData.Add(e.ID, e);
                }
            }

            else
            {
                Debug.LogError("Failed to load PatternData from ScriptableObject.");
            }
        }
        
        public static Dictionary<int, FamiliarData> DTFamiliarData = new Dictionary<int, FamiliarData>();

        public static void LoadFamiliarDataToGameData(string fileName)
        {
            string path = Consts.SCRIPTABLEOBJECT_LOAD_PATH + fileName;
            var familiarData = Resources.Load<ScriptableObject>(path) as FamiliarDataScriptableObject;

            if (familiarData != null)
            {
                foreach (var e in familiarData.familiarList)
                {
                    DTFamiliarData.Add(e.index, e);
                }
            }

            else
            {
                Debug.LogError("Failed to load familiarData from ScriptableObject.");
            }
        }
    }   
}
