using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GhostKnight : Familiar
{
    [SerializeField] private Image hpBar;
    private float fadeInTime = 0.5f;
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
        if (!TimeManager.Instance.IsCoolTimeFinished(gameObject.GetInstanceID().ToString()))
        {
            return;
        }
        animator.SetBool("Run", true);
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
        if (currentHp <= 0)
        {
            needDestroy = true;
        }

        return needDestroy;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CollisionManager.Instance.HandleCollision(this.gameObject, col.gameObject);
    }
    
    public override void HitMonster(Monster monster)
    {
        if (monster)
        {
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);
            monster.CurrentHp -= SkillManager.instance.PlayerResultSkillDamage(familiarData.skillId);
            currentHp -= monster.CurrentHp;
            hpBar.fillAmount = currentHp / familiarData.maxHp;
            monster.hpBar.fillAmount = monster.CurrentHp / monster.monsterData.MaxHP;
        }
    }

    protected override void Init()
    {
        base.Init();
        TimeManager.Instance.RegisterCoolTime(gameObject.GetInstanceID().ToString(), fadeInTime);
        TimeManager.Instance.Use(gameObject.GetInstanceID().ToString());
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        animator = GetComponent<Animator>();

        EffectUtility.FadeIn(spriteRenderers , fadeInTime);
        currentHp = familiarData.maxHp;
    }
}
