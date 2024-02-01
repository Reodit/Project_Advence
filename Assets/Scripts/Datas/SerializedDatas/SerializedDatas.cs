using System;
using System.Collections.Generic;
using System.Data;
using Enums;
using UnityEngine;

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
        SelectCriticalDamage
    }

    [Serializable]
    public enum EnchantEffect1
    {
        SkillDamageControl = 0,
        AttackSpeedControl,
        RangeControl,
        ProjectileSpeedControl,
        AddFrontProjectile,
        AddSlashProjectile
    }
    
    [Serializable]
    public enum MonsterType
    {
        Melee,
        Range,
        Trap,
        Boss
    }
}



[Serializable]
public class CharacterTable : IBaseData
{
    public string characterName;
    public int id;
    public int attackDamage; // 기본 스텟 공격력
    public int attackSpeed; // 천분, 초당 공격 속도
    public int defence;
    public float moveSpeed;
    public int criticalPercent; // 천분율 치명타 확률
    public int criticalDamage; // 천분율 치명타 데미지
    public int maxHp;
    public int maxLv;
    public string prefabPath;
    
    public void InitializeFromTableData(DataRow row)
    {
        this.characterName = row["Name"].ToString();
        this.id = Convert.ToInt32(row["ID"]);
        this.attackDamage = Convert.ToInt32(row["AttackDamage"]);
        this.attackSpeed = Convert.ToInt32(row["AttackSpeed"]);
        this.defence = Convert.ToInt32(row["Defence"]);
        this.moveSpeed = Convert.ToSingle(row["MoveSpeed"]);
        this.criticalPercent = Convert.ToInt32(row["CriticalPercent"]);
        this.criticalDamage = Convert.ToInt32(row["CriticalDamage"]);
        this.maxHp = Convert.ToInt32(row["HP"]);
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
    public int addStatValue;
    public int statLevel;
    public UpgradeStat upgradeName;
    public int requireGoldValue;
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.statLevel = Convert.ToInt32(row["Level"]);
        this.addStatValue = Convert.ToInt32(row["AddStat"]);
        this.requireGoldValue = Convert.ToInt32(row["Gold"]);
        this.upgradeName = (UpgradeStat)Enum.Parse(typeof(UpgradeStat), row["UpgradeStat"].ToString());
    }
}

[Serializable]
public class SelectStatTable : IBaseData
{
    public int index;
    public string name;
    public SelectStatName selectStatName;
    public int selectStatRateValue;
    public string description;
    public string iconPath;
    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.name = row["Name"].ToString();
        this.selectStatName = (SelectStatName)Enum.Parse(typeof(SelectStatName), row["SelectStatName"].ToString());
        this.selectStatRateValue = Convert.ToInt32(row["SelectStatRateValue"]);
        this.description = row["Description"].ToString();
        this.iconPath = row["Icon"].ToString();
    }
}

[Serializable]
public class SkillTable : IBaseData
{
    public int index;
    public string name;
    public string description;
    public string icon;
    public int skillDamageRate;
    public int skillSpeedRate;
    public int range;
    public int projectileSpeed;
    public int projectileSize;
    public string prefabPath;

    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.name = row["Name"].ToString();
        this.description = row["Description"].ToString();
        this.icon = row["Icon"].ToString();
        this.skillDamageRate = Convert.ToInt32(row["SkillDamageRate"]);
        this.skillSpeedRate = Convert.ToInt32(row["SkillSpeedRate"]);
        this.range = Convert.ToInt32(row["Range"]);
        this.projectileSpeed = Convert.ToInt32(row["ProjectileSpeed"]);
        this.projectileSize = Convert.ToInt32(row["ProjectileSize"]);
        this.prefabPath = row["Prefab"].ToString();
    }
}


[Serializable]
public class SkillEnchantTable : IBaseData
{
    public int index;
    public string name;
    public string description;
    public EnchantEffect1 enchantEffect1; // Enum 타입은 나중에 추가
    public int enchantEffectValue1;
    public int maxCnt;
    public string icon;
    public bool colorToBlack;

    public void InitializeFromTableData(DataRow row)
    {
        this.index = Convert.ToInt32(row["Index"]);
        this.name = row["Name"].ToString();
        this.description = row["Description"].ToString();
        this.enchantEffect1 = (EnchantEffect1)Enum.Parse(typeof(EnchantEffect1), row["EnchantEffect1"].ToString());
        this.enchantEffectValue1 = Convert.ToInt32(row["EnchantEffectValue1"]);
        this.maxCnt = Convert.ToInt32(row["MaxCnt"]);
        this.icon = row["Icon"].ToString();
        this.colorToBlack = Convert.ToBoolean(row["ColortoBlack"]);
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
    public int MaxHP;
    public int Attack;
    public int Defence;
    public int EXP;
    public int Gold;
    public int Score;
    public int Skill; // Skill은 여기서 문자열로 가정합니다. 필요에 따라 다른 타입으로 변경할 수 있습니다.
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
        this.Skill = Convert.ToInt32(row["Skill"]);
        this.PrefabPath = row["PrefabPath"].ToString();
    }
}

