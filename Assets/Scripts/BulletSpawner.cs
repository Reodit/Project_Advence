using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public Transform spawnPoint; // 발사 위치
    public string bulletPath;
    public List<Bullet> bullets;
    public void Start()
    {
        spawnPoint = this.transform;
        StartCoroutine(FireContinuously());
    }
    
    public float fireRate = 2f; // 초당 발사 횟수
    private float nextFireTime = 0f; // 다음 발사 시간

    IEnumerator FireContinuously()
    {
        // 무한 루프를 사용하여 지속적으로 총알을 발사
        while (true)
        {
            // 총알 인스턴스 생성
            if (GameManager.Instance.PlayerMove.playerSkills.ToList().Count > 0)
            {
                SpawnBullet(GameManager.Instance.PlayerMove.playerSkills.ToList()[0].Value.SkillTable.prefabPath);
            }
            // 다음 발사까지 기다림 (초당 발사 횟수의 역수를 기다림 시간으로 사용)
            yield return new WaitForSeconds(1f / fireRate);
        }
    }
    
    void SpawnBullet(string path)
    {
        GameObject bulletPrefab = Resources.Load<GameObject>(path);

        // 프리팹이 제대로 불러와졌는지 확인합니다.
        if (bulletPrefab != null)
        {
            var bulletObj = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            bullets.Add(bulletObj.GetComponent<Bullet>());
        }
        else
        {
            Debug.LogError("이름 틀림");
        }
    }
}
