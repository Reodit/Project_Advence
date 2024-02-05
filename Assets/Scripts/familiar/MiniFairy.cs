using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFairy : Familiar
{
    protected float moveSpeed;
    protected float attackDamage;
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
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x - moveSpeed * Time.deltaTime, currentPosition.y, 0f);
        transform.position = newPosition;
    }
    
    // Hit Logic & out of camera
    protected override bool CheckDestroyCondition()
    {
        bool needDestroy = base.CheckDestroyCondition() ||
                           CameraUtility.IsTargetInCameraView(GameManager.Instance.mainCamera,
                               this.transform.position);
        return needDestroy;
    }

    public override void HitMonster(Monster monster)
    {
        if (monster)
        {
            StartCoroutine(monster.FlashHitColor());
            monster.CurrentHp -= (int)attackDamage;
            monster.Hpbar.fillAmount = (float)monster.CurrentHp / monster.monsterMaxHp;
        }
    }
    
    // 초기화
    protected override void Init()
    {
        base.Init();
    }
}
