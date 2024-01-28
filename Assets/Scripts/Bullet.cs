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
    
    public void Start()
    {
        _isTriggered = false;
        destroyDelay = 0.5f;
        initPosition = transform.position;
        _pixelArsenalProjectileScript = transform.GetComponent<PixelArsenalProjectileScript>();
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
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x + speed * Time.deltaTime, currentPosition.y, 0f);
        transform.position = newPosition;
        
        if (Vector3.Distance(initPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _pixelArsenalProjectileScript.OnCol();
            var monster = other.GetComponent<Monster>();
            monster.Hit();
            monster.currentHp -= bulletDamage;
            monster.Hpbar.fillAmount = (float) monster.currentHp / monster.monsterMaxHp;
            TriggerDestruction();
        }
    }
}
