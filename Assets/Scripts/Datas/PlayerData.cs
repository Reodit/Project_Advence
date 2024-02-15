using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Datas
{
    public static class PlayerData
    {
        private static Dictionary<int, CharacterSkill> _characterSkills;        
        private static UpgradeHistory _upgradeHistory;
        private static CharacterStat _characterStat;

        private static readonly string CacheFilePath = Application.persistentDataPath + "/playerData.json";

        public static void LoadCharacterStatData()
        {
            if (!File.Exists(CacheFilePath))
            {
                InitializeCharacterSkillAndUpgradeHistory();
                _characterStat = new CharacterStat();
                return;
            }

            var jsonData = File.ReadAllText(CacheFilePath);
            InitializeCharacterSkillAndUpgradeHistory();
            _characterStat = JsonUtility.FromJson<CharacterStat>(jsonData);
        }

        public static void InitializeCharacterSkillAndUpgradeHistory()
        {
            _characterSkills = new Dictionary<int, CharacterSkill>();
            _upgradeHistory = new UpgradeHistory(null);
        }
    
        public static void SaveCharacterStatData()
        {
            File.WriteAllText(CacheFilePath, JsonUtility.ToJson(_characterStat));
        }

        public static Dictionary<int, CharacterSkill> GetCharacterSkills() => _characterSkills;
        public static void SetCharacterSkills(Dictionary<int, CharacterSkill> skills) => _characterSkills = skills;

        public static UpgradeHistory GetUpgradeHistory() => _upgradeHistory;
        public static void SetUpgradeHistory(UpgradeHistory history) => _upgradeHistory = history;

        public static CharacterStat GetCharacterStats() => _characterStat;
        public static void SetCharacterStats(CharacterStat stats) => _characterStat = stats;
    }
}