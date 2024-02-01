using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class BulletSpawner : MonoBehaviour
{
    public Transform spawnPoint; // 발사 위치
    public string bulletPath;
    public List<Bullet> bullets;

    // 파티클 에셋 사이즈 5 / 12 한 상황
    [SerializeField] private int bulletSpawnSpace = 5;
    [SerializeField] private int maxBulletSpawnArea = 12;
    
    private PlayerMove _player;
    private int _bulletSpawnAreaCount;

    private Dictionary<string, Bullet> _bulletPrefabDict = new Dictionary<string, Bullet>();
    
    private List<Bullet> _activeBullets = new List<Bullet>();

    private List<Bullet> _frontBullet = new List<Bullet>();
    private List<Bullet> _slashBullet = new List<Bullet>();

    private float _totalSpawnHeight;
    private float _bulletInterval;
    private float _bulletIntervalHalf;

    public void Start()
    {
        _player = GameManager.Instance.PlayerMove;
        _totalSpawnHeight = _player.transform.localScale.y * bulletSpawnSpace;

        _bulletInterval = _totalSpawnHeight / maxBulletSpawnArea;
        _bulletIntervalHalf = _bulletInterval * 0.5f;


        SkillManager.instance.OnAddSkill += AddSkillCallback;
        SkillManager.instance.OnAddEnchant += AddEnchantCallback;
        
        spawnPoint = this.transform;
        StartCoroutine(FireContinuously());
    }

    private void OnDestroy()
    {
        SkillManager.instance.OnAddSkill -= AddSkillCallback;
        SkillManager.instance.OnAddEnchant -= AddEnchantCallback;
    }

    public float fireRate = 2f; // 초당 발사 횟수
    private float nextFireTime = 0f; // 다음 발사 시간

    public void AddSkillCallback(SkillTable skill)
    {
        Bullet bullet = Resources.Load<Bullet>(skill.prefabPath);
        _bulletPrefabDict.Add(skill.name, bullet);
        if (_bulletPrefabDict.Count % 2 == 0)
        {
            _frontBullet.Add(bullet);
        }
        else
        {
            _frontBullet.Insert(0, bullet);
        }
        _bulletSpawnAreaCount++;
    }

    public void AddEnchantCallback(string skillName, SkillEnchantTable enchant)
    {
        switch (enchant.enchantEffect1)
        {
            case EnchantEffect1.SkillDamageControl:
                break;
            case EnchantEffect1.AttackSpeedControl:
                break;
            case EnchantEffect1.RangeControl:
                break;
            case EnchantEffect1.ProjectileSpeedControl:
                break;
            case EnchantEffect1.AddFrontProjectile:
                AddFrontBullet(skillName);
                //_slashBullet.Add(_bulletPrefabDict[skillName]);
                //_bulletSpawnAreaCount++;
                break;
            case EnchantEffect1.AddSlashProjectile:
                AddFrontBullet(skillName);
                break;
        }
    }

    private void AddFrontBullet(string skillName)
    {
        int index = _frontBullet.IndexOf(_bulletPrefabDict[skillName]);
        if (index != -1)
        {
            _frontBullet.Insert(index, _bulletPrefabDict[skillName]);
        }
        else
        {
            _frontBullet.Add(_bulletPrefabDict[skillName]);
        }
        _bulletSpawnAreaCount++;
    }

    IEnumerator FireContinuously()
    {
        // 무한 루프를 사용하여 지속적으로 총알을 발사
        while (true)
        {
            // 총알 인스턴스 생성
            if (_bulletPrefabDict.Count > 0)
            {
                int frontBulletCount = _frontBullet.Count;
                int halfCount = frontBulletCount / 2;
                if (frontBulletCount % 2 == 0)
                {
                    for (int i = 0; i < frontBulletCount; i++)
                    {
                        Vector2 spawnPos = spawnPoint.position;
                        spawnPos.y += (i - halfCount + 1) * _bulletInterval - _bulletIntervalHalf;
                        SpawnBullet(_frontBullet[i], spawnPos);
                    }
                }
                else
                {
                    for (int i = 0; i < frontBulletCount; i++)
                    {
                        Vector2 spawnPos = spawnPoint.position;
                        spawnPos.y += (i - halfCount) * _bulletInterval;
                        SpawnBullet(_frontBullet[i], spawnPos);
                    }
                }
            }
            // 다음 발사까지 기다림 (초당 발사 횟수의 역수를 기다림 시간으로 사용)
            yield return new WaitForSeconds(1f / fireRate);
        }
    }
    
    void SpawnBullet(Bullet bulletPrefab, Vector2 pos)
    {
        // 프리팹이 제대로 불러와졌는지 확인합니다.
        if (bulletPrefab != null)
        {
            Bullet bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
            bullet.Init(RemoveBullet);
            _activeBullets.Add(bullet);
        }
    }

    private void RemoveBullet(Bullet bullet)
    {
        _activeBullets.Remove(bullet);
    }
}