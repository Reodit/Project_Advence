using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Datas
{
    public static class PlayerData
    {
        private static Dictionary<int, CharacterSkill> _characterSkills;        
        private static UpgradeHistory _upgradeHistory;
        private static List<StatLevelTable> _characterShopStat;
        
        private static readonly string CacheFilePath = Application.persistentDataPath + "/playerData.json";

        public static void LoadCharacterStatData()
        {
            if (!File.Exists(CacheFilePath))
            {
                InitializeCharacterSkillAndUpgradeHistory();
                _characterShopStat = new List<StatLevelTable>();
                return;
            }

            var jsonData = File.ReadAllText(CacheFilePath);
            InitializeCharacterSkillAndUpgradeHistory();
            _characterShopStat = JsonUtility.FromJson<List<StatLevelTable>>(jsonData);
        }

        public static void InitializeCharacterSkillAndUpgradeHistory()
        {
            _characterSkills = new Dictionary<int, CharacterSkill>();
            _upgradeHistory = new UpgradeHistory(
                new List<SelectStatTable>());
        }
    
        public static void SaveCharacterStatData()
        {
            File.WriteAllText(CacheFilePath, JsonUtility.ToJson(_characterShopStat));
        }

        public static Dictionary<int, CharacterSkill> GetCharacterSkills() => _characterSkills;
        public static void SetCharacterSkills(Dictionary<int, CharacterSkill> skills) => _characterSkills = skills;

        public static UpgradeHistory GetUpgradeHistory() => _upgradeHistory;
        public static void SetUpgradeHistory(UpgradeHistory history) => _upgradeHistory = history;

        public static List<StatLevelTable> GetCharacterStats() => _characterShopStat;
        public static void SetCharacterStats(List<StatLevelTable> shopStats) => _characterShopStat = shopStats;
    }
}