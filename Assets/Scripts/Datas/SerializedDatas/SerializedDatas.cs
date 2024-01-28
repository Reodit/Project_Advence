using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[Serializable]
public class CharacterTableScriptableObject : ScriptableObject
{
    public List<CharacterTable> CharacterTableList;
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
