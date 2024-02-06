using System;
using System.Collections;
using UnityEngine;

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
    private Vector3 initPosition;
    private CircleCollider2D _collider;

    private Action<Bullet> OnDestroyed;
    [field: SerializeField] public BulletInfo BulletInfo { get; private set; }

    public string SkillName { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    public void Start()
    {
        _isTriggered = false;
        destroyDelay = 0.5f;
        initPosition = transform.position;
        _pixelArsenalProjectileScript = transform.GetComponent<PixelArsenalProjectileScript>();
    }

    public void Init(BulletInfo bulletInfo, Action<Bullet> destroyCallback)
    {
        BulletInfo = bulletInfo;
        OnDestroyed = destroyCallback;
    }

    private void OnDestroy()
    {
        OnDestroyed.Invoke(this);
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
    
    IEnumerator DestroyAfterDelayCoroutine()
    {
        // destroyDelay 만큼 대기
        yield return new WaitForSeconds(destroyDelay);

        // 오브젝트 삭제
        Destroy(gameObject);
    }
    
    public void Update()
    {
        Vector2 newPosition = transform.position + transform.right * (BulletInfo.Speed * Time.deltaTime);
        transform.position = newPosition;
        
        if (Vector3.Distance(initPosition, transform.position) >= BulletInfo.MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionManager.Instance.HandleCollision(this.gameObject, collision.gameObject);
    }

    public void HitMonster(Monster monster)
    {
        if (monster)
        {
            _pixelArsenalProjectileScript.OnCol();
            monster.FlashHitColor();
            
            // TODO 몬스터 데미지 계산 통일필요
            monster.CurrentHp -= (int)BulletInfo.Damage;
            monster.Hpbar.fillAmount = (float)monster.CurrentHp / monster.monsterMaxHp;
            TriggerDestruction();
        }
    }
    
    public void Resize(float amount)
    {
        _collider.radius += _collider.radius * amount;
    }
}
