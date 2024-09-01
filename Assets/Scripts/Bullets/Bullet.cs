using System;
using System.Linq;
using UnityEngine;
using Enums;

public class Bullet : MonoBehaviour
{
    public PixelArsenalProjectileScript PixelArsenalProjectileScript { get; private set; }
    protected Vector3 InitPosition;
    public bool isFamiliarBullet;
    private Action<Bullet> onDestroyed;
    [field: SerializeField] public BulletInfo BulletInfo { get; protected set; }
    [field: SerializeField] public int SkillIndex { get; private set; }
    public SkillType SkillType { get; private set; } = SkillType.Normal;
    public Vector3 worldPosition;
    protected virtual void Start()
    {
        PixelArsenalProjectileScript = transform.GetComponent<PixelArsenalProjectileScript>();
    }

    public virtual void Init(BulletInfo bulletInfo, Action<Bullet> destroyCallback, int skillIndex)
    {
        this.SkillIndex = skillIndex;
        BulletInfo = bulletInfo;
        onDestroyed = destroyCallback;
        InitPosition = transform.position;
        worldPosition = transform.position;
    }

    public virtual void UpdateBulletPosition()
    {
        worldPosition += Vector3.right * (BulletInfo.speed * Time.deltaTime);
        transform.position = worldPosition;
        
        if (Vector3.Distance(InitPosition, worldPosition) >= BulletInfo.maxDistance)
        {
            RemoveBullet();
        }
    }
    
    public void SetSkillIndex(int skillIndex)
    {
        SkillIndex = skillIndex;
    }

    public void SetBulletInfo(BulletInfo bulletInfo)
    {
        BulletInfo = bulletInfo;
    }

    public virtual void RemoveBullet()
    {
        ObjectPoolManager.instance.ReturnToPool("Bullet",
            BulletInfo.bulletPrefabPath, this.gameObject);
    }
    
    protected virtual void Update()
    {
        UpdateBulletPosition();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionManager.Instance.HandleCollision(this.gameObject, collision.gameObject);
    }

    public virtual void HitMonster(Monster monster)
    {
        if (monster)
        {
            PixelArsenalProjectileScript.OnCol();
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);

            // TODO 몬스터 데미지 계산 통일필요
            ApplyDamage(monster);
            RemoveBullet();
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
}
