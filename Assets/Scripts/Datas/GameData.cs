
using System.Collections.Generic;
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
    }   
}
