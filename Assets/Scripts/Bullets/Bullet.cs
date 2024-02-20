using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Enums;

public class Bullet : MonoBehaviour
{
    public Transform spawnPoint;
    public PixelArsenalProjectileScript pixelArsenalProjectileScript { get; private set; }
    private bool _isTriggered;
    private float destroyDelay;
    protected Vector3 initPosition;
    public bool isFamiliarBullet;
    private Action<Bullet> OnDestroyed;
    [field: SerializeField] public BulletInfo BulletInfo { get; protected set; }

    public int SkillIndex { get; private set; }

    public SkillType SkillType { get; private set; } = SkillType.Normal;

    protected Transform myTrans;

    protected virtual void Awake()
    {
        myTrans = transform;
    }

    protected virtual void Start()
    {
        initPosition = transform.position;
        _isTriggered = false;
        destroyDelay = 0.5f;
        pixelArsenalProjectileScript = transform.GetComponent<PixelArsenalProjectileScript>();
    }

    protected virtual void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public virtual void Init(BulletInfo bulletInfo, Action<Bullet> destroyCallback, int skillIndex)
    {
        this.SkillIndex = skillIndex;
        BulletInfo = bulletInfo;
        OnDestroyed = destroyCallback;
    }


    public void SetSkillIndex(int skillIndex)
    {
        SkillIndex = skillIndex;
    }

    public void SetBulletInfo(BulletInfo bulletInfo)
    {
        BulletInfo = bulletInfo;
    }

    public void TriggerDestruction()
    {
        if (!_isTriggered)
        {
            _isTriggered = true;
            StartCoroutine(DestroyAfterDelayCoroutine());
        }
    }

    protected virtual IEnumerator DestroyAfterDelayCoroutine()
    {
        // destroyDelay 만큼 대기
        yield return new WaitForSeconds(destroyDelay);

        // 오브젝트 삭제
        Destroy(gameObject);
    }
    
    protected virtual void Update()
    {
        Vector2 newPosition = transform.position + transform.right * (BulletInfo.Speed * Time.deltaTime);
        transform.position = newPosition;
        
        if (Vector3.Distance(initPosition, transform.position) >= BulletInfo.MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionManager.Instance.HandleCollision(this.gameObject, collision.gameObject);
    }

    public virtual void HitMonster(Monster monster)
    {
        if (monster)
        {
            pixelArsenalProjectileScript.OnCol();
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);

            // TODO 몬스터 데미지 계산 통일필요
            ApplyDamage(monster);
            TriggerDestruction();
        }
    }

    protected void ApplyDamage(Monster monster)
    {
        monster.CurrentHp -= isFamiliarBullet ?
                        SkillManager.instance.PlayerResultSkillDamage(Datas.GameData.DTFamiliarData.FirstOrDefault(x =>
                            x.Value.familiarSkillId == SkillIndex).Value.skillId) :
                        SkillManager.instance.PlayerResultSkillDamage(SkillIndex);
        monster.hpBar.fillAmount = (float)monster.CurrentHp / monster.monsterData.MaxHP;
    }

    public virtual void HitPlayer(Monster monster, PlayerMove player)
    {

    }
    
    public void Resize(float amount)
    {
        Vector3 scale = myTrans.localScale;
        scale += scale * amount;
        myTrans.localScale = scale;
    }

    public void SetSkillType(SkillType skillType)
    {
        SkillType = SkillType;
    }
}
