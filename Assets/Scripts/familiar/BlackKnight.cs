using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlackKnight : Familiar
{
    [SerializeField] private Image hpBar;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        Vector3 currentPosition = transform.root.position;
        Vector3 newPosition = new Vector3(currentPosition.x + familiarData.MoveSpeed * Time.deltaTime, currentPosition.y, 0f);
        transform.root.position = newPosition;
    }
    
    // Hit Logic & out of camera
    protected override bool CheckDestroyCondition()
    {
        bool needDestroy = base.CheckDestroyCondition() || 
                           CameraUtility.IsTargetInCameraView(GameManager.Instance.mainCamera,
                               this.transform.position);
        if (currentHp <= 0)
        {
            needDestroy = true;
        }

        return needDestroy;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Test");
        CollisionManager.Instance.HandleCollision(this.gameObject, col.gameObject);
    }
    
    public override void HitMonster(Monster monster)
    {
        if (monster)
        {
            monster.FlashHitColor();
            monster.CurrentHp -= familiarData.MaxHp * familiarSkillData.skillDamageRate;
            currentHp -= monster.attackDamage;
            hpBar.fillAmount = (float)currentHp / familiarData.MaxHp;
            monster.Hpbar.fillAmount = (float)monster.CurrentHp / monster.monsterMaxHp;
        }
    }
    // 초기화
    protected override void Init()
    {
        base.Init();
        currentHp = familiarData.MaxHp;
    }
}
