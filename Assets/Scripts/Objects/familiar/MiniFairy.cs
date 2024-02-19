using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFairy : Familiar
{
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
        Vector3 newPosition = new Vector3(currentPosition.x + familiarData.moveSpeed * Time.deltaTime, currentPosition.y, 0f);
        transform.root.position = newPosition;
    }
    
    // Hit Logic & out of camera
    protected override bool CheckDestroyCondition()
    {
        bool needDestroy = base.CheckDestroyCondition() ||
                           !CameraUtility.IsTargetInCameraView(GameManager.Instance.mainCamera,
                               this.transform.position);
        return needDestroy;
    }

    public override void HitMonster(Monster monster)
    {
        if (monster)
        {
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);
            monster.CurrentHp -= SkillManager.instance.PlayerResultSkillDamage(familiarData.skillId);
            monster.hpBar.fillAmount = (float)monster.CurrentHp / monster.monsterData.MaxHP;
        }
    }
    
    // 초기화
    protected override void Init()
    {
        base.Init();
        animator = GetComponent<Animator>();
        bulletController = GetComponent<BulletController>();
        bulletController.OnFire += PlayFireAnimation;
        bulletController.AddSkillCallback(Datas.GameData.DTSkillData[familiarData.familiarSkillId]);
    }

    private void PlayFireAnimation()
    {
        animator.Play("MagicAttack");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        bulletController.OnFire -= PlayFireAnimation;
    }
    
    public void TurnOffAttack()
    {
        
    }

    public void MagicAttack()
    {
        
    }

    public void RangedAttack()
    {
        
    }
}
