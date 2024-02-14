using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class CharacterSkill
{
    public SkillTable SkillTable;
    public List<SkillEnchantTable> SkillEnchantTables;
    public CharacterSkill(SkillTable skillTable, List<SkillEnchantTable> skillEnchantTable)
    {
        this.SkillTable = skillTable;
        this.SkillEnchantTables = skillEnchantTable;
    }
}

[Serializable]
public class UpgradeHistory
{
    public List<SelectStatTable> SelectStatTable;

    public UpgradeHistory(List<SelectStatTable> selectStatTable)
    {
        this.SelectStatTable = selectStatTable;
    }
}

public class PlayerMove : MonoBehaviour
{
    public int currentHp;
    public Image Hpbar;
    
    public int currentExp;
    public int currentLvl;
    public bool isHit = false;
    public CharacterTable characterData;
    public Color hitColor = Color.red;
    public float hitDuration = 0.2f;
    public List<SpriteRenderer> spriteRenderers;

    void Start()
    {
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }
    
    void Update()
    {
        Move();
        LevelUp();
    }

    public void LevelUp()
    {
        if (currentLvl >= characterData.maxLv)
        {
            currentExp = Datas.GameData.DTCharacterLevelData[characterData.maxLv].reqExp;
            currentLvl = characterData.maxLv;
            return;
        }
        
        if (Datas.GameData.DTCharacterLevelData[currentLvl].reqExp <= currentExp)
        {
            currentExp -= Datas.GameData.DTCharacterLevelData[currentLvl].reqExp;
            currentLvl++;
            PopupManager.Instance.InstantiatePopUp("UIPrefabs/LevelUpPopUp");
        }
    }

    public void Init()
    {
        currentLvl = 0;
        currentExp = 0;
        currentHp = characterData.maxHp;
    }
    
    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0f) * (characterData.moveSpeed * Time.deltaTime);

        // MoveArea 컴포넌트를 사용하여 새 위치가 이동 영역 내에 있는지 확인
        newPosition = GameManager.Instance.MoveArea.ConstrainPosition(newPosition);

        transform.position = newPosition;
    }
}
