using System;
using System.Collections;
using UnityEngine;
using Enums;

[Serializable]
public struct BulletInfo
{
    [field: SerializeField] public float Damage;
    [field: SerializeField] public float MaxDistance;
    [field: SerializeField] public float Speed; // 총알의 속도

    public BulletInfo(float damage, float maxDistance, float speed)
    {
        Damage = damage;
        MaxDistance = maxDistance;
        Speed = speed;
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    public void SetMaxDistance(float maxDistance)
    {
        MaxDistance = maxDistance;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }
}

public class Bullet : MonoBehaviour
{
    public Transform spawnPoint;
    public PixelArsenalProjectileScript _pixelArsenalProjectileScript { get; private set; }
    private bool _isTriggered;
    private float destroyDelay;
    protected Vector3 initPosition;

    private Action<Bullet> OnDestroyed;
    [field: SerializeField] public BulletInfo BulletInfo { get; private set; }

    public string SkillName { get; private set; }

    public SkillType SkillType { get; private set; } = SkillType.Normal;

    protected Transform _myTrans;

    private void Awake()
    {
        _myTrans = transform;
    }

    protected void Start()
    {
        initPosition = transform.position;
        _isTriggered = false;
        destroyDelay = 0.5f;
        _pixelArsenalProjectileScript = transform.GetComponent<PixelArsenalProjectileScript>();
    }

    private void OnDestroy()
    {
        OnDestroyed.Invoke(this);
    }

    public void Init(BulletInfo bulletInfo, Action<Bullet> destroyCallback)
    {
        BulletInfo = bulletInfo;
        OnDestroyed = destroyCallback;
    }


    public void SetSkillName(string skillName)
    {
        SkillName = skillName;
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
    
    protected void Update()
    {
        Vector2 newPosition = transform.position + transform.right * (BulletInfo.Speed * Time.deltaTime);
        transform.position = newPosition;
        
        if (Vector3.Distance(initPosition, transform.position) >= BulletInfo.MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionManager.Instance.HandleCollision(this.gameObject, collision.gameObject);
    }

    public virtual void HitMonster(Monster monster)
    {
        if (monster)
        {
            _pixelArsenalProjectileScript.OnCol();
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);
            
            // TODO 몬스터 데미지 계산 통일필요
            monster.CurrentHp -= (int)BulletInfo.Damage;
            monster.hpBar.fillAmount = (float)monster.CurrentHp / monster.monsterData.MaxHP;
            TriggerDestruction();
        }
    }

    public virtual void HitPlayer(Monster monster, PlayerMove player)
    {

    }
    
    public void Resize(float amount)
    {
        Vector3 scale = _myTrans.localScale;
        scale += scale * amount;
        _myTrans.localScale = scale;
    }

    public void SetSkillType(SkillType skillType)
    {
        SkillType = SkillType;
    }
}
