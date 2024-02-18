using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float currentHp;
    public Image Hpbar;
    
    public int currentExp;
    public int currentLvl;
    public CharacterTable characterData;
    public Color hitColor = Color.red;
    public float hitDuration = 0.2f;
    public List<SpriteRenderer> spriteRenderers;

#if UNITY_EDITOR
    public bool isLevelUpOn = true;
#endif

    void Start()
    {
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }
    
    void Update()
    {
        Move();
#if UNITY_EDITOR
        if (!isLevelUpOn)
            return;
#endif
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
