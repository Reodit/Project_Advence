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
        CriticalChance
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
        RangeControl
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
