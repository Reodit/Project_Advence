using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform spawnPoint;
    public float speed = 5f; // 총알의 속도
    public float maxDistance = 10f;
    private PixelArsenalProjectileScript _pixelArsenalProjectileScript;
    private bool _isTriggered;
    private float destroyDelay;
    public int bulletDamage;
    private Vector3 initPosition;
    private CircleCollider2D _collider;

    private Action<Bullet> OnDestroyed;

    private string _skillName;

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

    public void Init(Action<Bullet> destroyCallback)
    {
        OnDestroyed = destroyCallback;
    }

    private void OnDestroy()
    {
        OnDestroyed.Invoke(this);
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
        Vector2 newPosition = transform.position + transform.right * speed * Time.deltaTime;
        transform.position = newPosition;
        
        if (Vector3.Distance(initPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            _pixelArsenalProjectileScript.OnCol();
            monster.Hit();
            monster.currentHp -= bulletDamage;
            monster.Hpbar.fillAmount = (float)monster.currentHp / monster.monsterMaxHp;
            TriggerDestruction();
        }
    }

    public void Resize(float amount)
    {
        _collider.radius += _collider.radius * amount;
    }
}