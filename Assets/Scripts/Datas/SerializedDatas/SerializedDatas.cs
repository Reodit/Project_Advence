using System;
using System.Collections.Generic;
using System.Data;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enums
{
    [Serializable]
    public enum UpgradeStat
    {
        AttackDamage = 0,
        AttackSpeed,
        MoveSpeed,
        Defence,
        CriticalChance,
        Health
    }

    [Serializable]
    public enum SelectStatName
    {
        SelectDamage = 0,
        SelectAttackSpeed,
        SelectMoveSpeed,
        SelectDefence,
        SelectCriticalDamage,
        SelectHealthRegen,
        SelectHealth
    }

    [Serializable]
    public enum EnchantEffect1
    {
        SkillDamageControl = 0,
        AttackSpeedControl,
        RangeControl,
        ProjectileSpeedControl,
        AddSlashProjectile,
        AddFrontProjectile,
        CriticalDamageControl,
        FamiliarHealthControl
    }
    
    [Serializable]
    public enum MonsterType
    {
        Melee,
        Range,
        Trap,
        Boss
    }

    [Serializable]
    public enum SkillType
    {
        Normal,
        Waterballoon,
        Outside,
        MeleeFamiliar,
        RangeFamiliar
    }
}



[Serializable]
public class CharacterTable : IBaseData
{
    public string characterName;
    public int id;
    public float attackDamage;
    public float attackSpeed;
    public float defence;
    public float moveSpeed;
    public float criticalPercent;
    public float criticalDamage;
    public float maxHp;
    public int maxLv;
    public string prefabPath;
    
    public void InitializeFromTableData(DataRow row)
    {
        this.characterName = row["Name"].ToString();
        this.id = Convert.ToInt32(row["ID"]);
        this.attackDamage = Convert.ToSingle(row["AttackDamage"]);
        this.attackSpeed = Convert.ToSingle(row["AttackSpeed"]);
        this.defence = Convert.ToSingle(row["Defence"]);
        this.moveSpeed = Convert.ToSingle(row["MoveSpeed"]);
        this.criticalPercent = Convert.ToSingle(row["CriticalPercent"]);
        this.criticalDamage = Convert.ToSingle(row["CriticalDamage"]);
        this.maxHp = Convert.ToSingle(row["HP"]);
        this.maxLv = Convert.ToInt32(row["MaxLV"]);
        this.prefabPath = row["Prefab"].ToString();
    }
}

[Serializable]
public class CharacterLevelTable : IBaseData
{
    public int index;
    public int level;
    public int reqExp; 
    
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.level = Convert.ToInt32(row["Level"]);
        this.reqExp = Convert.ToInt32(row["ReqExp"]);
    }
}

[Serializable]
public class StatLevelTable : IBaseData
{
    public int index;
    public float addStatValue;
    public int statLevel;
    public UpgradeStat upgradeName;
    public int requireGoldValue;
    public string iconPath;
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.statLevel = Convert.ToInt32(row["Level"]);
        this.addStatValue = Convert.ToSingle(row["AddStat"]);
        this.requireGoldValue = Convert.ToInt32(row["Gold"]);
        this.upgradeName = (UpgradeStat)Enum.Parse(typeof(UpgradeStat), row["UpgradeStat"].ToString());
        this.iconPath = row["Icon"].ToString();
    }
}

[Serializable]
public class SelectStatTable : IBaseData
{
    public int index;
    public string name;
    public SelectStatName selectStatName;
    public float selectStatRateValue;
    public string description;
    public string iconPath;
    public int maxCount;
    public int currentUpgradeCount;
    
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.name = row["Name"].ToString();
        this.selectStatName = (SelectStatName)Enum.Parse(typeof(SelectStatName), row["SelectStatName"].ToString());
        this.selectStatRateValue = Convert.ToSingle(row["SelectStatRateValue"]);
        this.description = row["Description"].ToString();
        this.iconPath = row["Icon"].ToString();
        this.maxCount = Convert.ToInt32(row["MaxCount"]);
        this.currentUpgradeCount = 0;
    }
}

[Serializable]
public class SkillTable : IBaseData
{
    public int index;
    public string name;
    public string description;
    public string icon;
    public float skillDamageRate;
    public float skillSpeedRate;
    public float range;
    public float projectileSpeed;
    public float projectileSize;
    public string prefabPath;
    public SkillType type;
    public List<int> allowSkillEnchantKey;
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.name = row["Name"].ToString();
        this.description = row["Description"].ToString();
        this.icon = row["Icon"].ToString();
        this.skillDamageRate = Convert.ToSingle(row["SkillDamageRate"]);
        this.skillSpeedRate = Convert.ToSingle(row["SkillSpeedRate"]);
        this.range = Convert.ToSingle(row["Range"]);
        this.projectileSpeed = Convert.ToSingle(row["ProjectileSpeed"]);
        this.projectileSize = Convert.ToSingle(row["ProjectileSize"]);
        this.prefabPath = row["Prefab"].ToString();
        this.type = (SkillType)Enum.Parse(typeof(SkillType), row["Type"].ToString());
        this.allowSkillEnchantKey = row["AllowSkillEnchantKey"] as List<int>;
    }
}


[Serializable]
public class SkillEnchantTable : IBaseData
{
    public int index;
    public string name;
    public string description;
    public EnchantEffect1 enchantEffect1; // Enum 타입은 나중에 추가
    public float enchantEffectValue1;
    public int maxCnt;
    public string icon;
    public int currentCount;

    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.name = row["Name"].ToString();
        this.description = row["Description"].ToString();
        this.enchantEffect1 = (EnchantEffect1)Enum.Parse(typeof(EnchantEffect1), row["EnchantEffect1"].ToString());
        this.enchantEffectValue1 = Convert.ToSingle(row["EnchantEffectValue1"]);
        this.maxCnt = Convert.ToInt32(row["MaxCnt"]);
        this.icon = row["Icon"].ToString();
    }
}

[Serializable]
public class PhaseTable : IBaseData // IBaseData 인터페이스 구현 가정
{
    public int index;
    public int stage;
    public string backgroundLocal;
    public int scrollSpeed;
    public int firstPrintMonster;
    public int targetMonsterValue;
    public float phaseValue1;
    public float phaseValue2;
    public int phaseNumber;
    
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.stage = Convert.ToInt32(row["Stage"]);
        this.backgroundLocal = row["Background_Local"].ToString();
        this.scrollSpeed = Convert.ToInt32(row["ScrollSpeed"]);
        this.firstPrintMonster = Convert.ToInt32(row["FirstPrintMonster"]);
        this.targetMonsterValue = Convert.ToInt32(row["TargetMonsterValue"]);
        this.phaseValue1 = Convert.ToSingle(row["PhaseValue1"]);
        this.phaseValue2 = Convert.ToSingle(row["PhaseValue2"]);
        this.phaseNumber = Convert.ToInt32(row["PhaseNumber"]);
    }
}

[Serializable]
public class PatternTable : IBaseData
{
    public int index;
    public int vertical1;
    public int vertical2;
    public int vertical3;
    public int vertical4;
    public int vertical5;
    public int patternInterval;
    public int monsterCnt;

    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.vertical1 = Convert.ToInt32(row["Vertical1"]);
        this.vertical2 = Convert.ToInt32(row["Vertical2"]);
        this.vertical3 = Convert.ToInt32(row["Vertical3"]);
        this.vertical4 = Convert.ToInt32(row["Vertical4"]);
        this.vertical5 = Convert.ToInt32(row["Vertical5"]);
        this.patternInterval = Convert.ToInt32(row["PatternInterval"]);
        this.monsterCnt = Convert.ToInt32(row["MonsterCnt"]);
    }
}



[Serializable]
public class MonsterTable : IBaseData
{
    public int ID;
    public string Name;
    public MonsterType Type; // Enum 타입
    public float MaxHP;
    public float Attack;
    public float Defence;
    public int EXP;
    public int Gold;
    public int Score;
    public string PrefabPath;

    public void InitializeFromTableData(DataRow row)
    {
        this.ID = Convert.ToInt32(row["ID"]);
        this.Name = row["Name"].ToString();
        this.Type = (MonsterType)Enum.Parse(typeof(MonsterType), row["MonsterType"].ToString());
        this.MaxHP = Convert.ToInt32(row["MaxHP"]);
        this.Attack = Convert.ToInt32(row["Attack"]);
        this.Defence = Convert.ToInt32(row["Defence"]);
        this.EXP = Convert.ToInt32(row["EXP"]);
        this.Gold = Convert.ToInt32(row["Gold"]);
        this.Score = Convert.ToInt32(row["Score"]);
        this.PrefabPath = row["PrefabPath"].ToString();
    }
}

[Serializable]
public class FamiliarData : IBaseData
{
    [FormerlySerializedAs("Index")] public int index;
    [FormerlySerializedAs("SkillId")] public int skillId;
    [FormerlySerializedAs("MaxHp")] public float maxHp;
    [FormerlySerializedAs("MoveSpeed")] public float moveSpeed;
    public int familiarSkillId;

    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.skillId = Convert.ToInt32(row["SkillId"]);
        this.maxHp = Convert.ToSingle(row["MaxHp"]);
        this.moveSpeed = Convert.ToSingle(row["MoveSpeed"]);
        this.familiarSkillId = Convert.ToInt32(row["FamiliarSkillId"]);
    }
}

[Serializable]
public class CharacterSkill
{
    public SkillTable skillTable;
    public List<SkillEnchantTable> skillEnchantTables;
    public CharacterSkill(SkillTable skillTable, List<SkillEnchantTable> skillEnchantTable)
    {
        this.skillTable = skillTable;
        skillEnchantTables = skillEnchantTable;
    }
}

[Serializable]
public class UpgradeHistory
{
    public List<SelectStatTable> selectStatTable;

    public UpgradeHistory(List<SelectStatTable> selectStatTable)
    {
        this.selectStatTable = selectStatTable;
    }
}

[Serializable]
public class CharacterStat
{
    public int attackPoint;
    public float attackRate;
    public float criticalRate;
    public int healthPoint;
    public int defencePoint;
    public float moveSpeed;
}