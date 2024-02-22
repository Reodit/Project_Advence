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
    public FixedJoystick FixedJoystick { get; set; } 
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
        FixedJoystick = GameManager.Instance.fixedoystick;
    }
    
    void Move()
    {
        if (FixedJoystick == null)
        {
            return;
        }
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 newPosition = transform.position + new Vector3(
            FixedJoystick.Horizontal + moveX, FixedJoystick.Vertical + moveY, 0f) * 
            (characterData.moveSpeed * Time.deltaTime);

        newPosition = GameManager.Instance.MoveArea.ConstrainPosition(newPosition);

        transform.position = newPosition;
    }
}
