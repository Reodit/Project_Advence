using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform spawnPoint;
    public float speed = 5f; // 총알의 속도
    private float maxDistance = 10f;
    private PixelArsenalProjectileScript _pixelArsenalProjectileScript;
    private bool _isTriggered;
    private float destroyDelay;
    public void Start()
    {
        _isTriggered = false;
        destroyDelay = 0.5f;
        _pixelArsenalProjectileScript = transform.GetChild(0).GetComponent<PixelArsenalProjectileScript>();
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
        if (_isTriggered)
        {
            
        }
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x + speed * Time.deltaTime, currentPosition.y, 0f);
        transform.position = newPosition;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }
        _pixelArsenalProjectileScript.OnCol();
        other.GetComponent<Monster>().Hit();
        TriggerDestruction();
    }
}
