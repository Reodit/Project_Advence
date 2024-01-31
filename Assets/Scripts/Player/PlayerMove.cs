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
    
    private Coroutine flashCoroutine = null; // 코루틴의 참조를 저장할 변수
    public CharacterTable characterData;

    public Dictionary<string, CharacterSkill> playerSkills = new Dictionary<string, CharacterSkill>();
    public UpgradeHistory playerUpgradeHistory = new UpgradeHistory(new List<SelectStatTable>());
    void Update()
    {
        Move();
        LevelUp();
        if (isHit)
        {
            Hit();
        }
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
            PopupManager.Instance.InstantiateItemListPopUp("UIPrefabs/LevelUpPopUp");
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
    
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }
    
    public Color hitColor = Color.red; // 피격 시 적용할 색상
    public float hitDuration = 0.2f; // 피격 색상이 지속되는 시간

    // 피격 함수
    public void Hit()
    {
        isHit = false;
        if (flashCoroutine == null) // 코루틴이 실행 중이 아니라면 코루틴 실행
        {
            flashCoroutine = StartCoroutine(FlashHitColor());
        }    
    }

    // 피격 시 색상 변경 코루틴
    IEnumerator FlashHitColor()
    {
        // 원래 색상을 저장할 리스트
        List<Color> originalColors = new List<Color>();

        // 각 스프라이트 렌더러의 원래 색상을 저장하고 피격 색상으로 변경
        foreach (var spriteRenderer in spriteRenderers)
        {
            originalColors.Add(spriteRenderer.color);
            spriteRenderer.color = hitColor;
        }

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(hitDuration);

        // 각 스프라이트 렌더러의 색상을 원래 색상으로 복원
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }
        
        flashCoroutine = null; // 코루틴이 끝났음을 표시
    }
}
