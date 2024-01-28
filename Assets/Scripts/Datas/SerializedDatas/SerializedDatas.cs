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