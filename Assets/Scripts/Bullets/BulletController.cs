using Enums;
using NPOI.POIFS.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform spawnPoint; // 발사 위치
    public string bulletPath;
    public List<Bullet> bullets;

    // 파티클 에셋 사이즈 3 / 12 한 상황
    [SerializeField] private int bulletSpawnSpace = 3;
    [SerializeField] private int maxBulletSpawnArea = 12;

    [field: SerializeField] public float Angle { get; private set; } = 60f;
    
    private PlayerMove _player;
    private int _bulletSpawnAreaCount;

    private Dictionary<string, Bullet> _bulletPrefabDict = new Dictionary<string, Bullet>();
    
    private List<Bullet> _activeBullets = new List<Bullet>();

    private List<Bullet> _frontBullets = new List<Bullet>();
    private List<Bullet> _slashBullets = new List<Bullet>();

    private BulletSpawner _spawner;

    private const float PERCENT_DIVISION = 0.01f;

    public void Start()
    {
        _player = GameManager.Instance.PlayerMove;

        float bulletInterval = (float)bulletSpawnSpace / maxBulletSpawnArea;
        float bulletIntervalHalf = bulletInterval * 0.5f;

        spawnPoint = transform;

        _spawner = new BulletSpawner(spawnPoint, bulletInterval, bulletIntervalHalf, AddActiveBulletCallback, RemoveActiveBulletCallback);

        SkillManager.instance.OnAddSkill += AddSkillCallback;
        SkillManager.instance.OnAddEnchant += AddEnchantCallback;
        
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
        AddFirstBulletType(bullet);
    }

    public void AddEnchantCallback(string skillName, SkillEnchantTable enchant)
    {
        switch (enchant.enchantEffect1)
        {
            case EnchantEffect1.SkillDamageControl:
                IncreaseSkillDamage(skillName, enchant.index);
                break;
            case EnchantEffect1.AttackSpeedControl:
                break;
            case EnchantEffect1.RangeControl:
                break;
            case EnchantEffect1.ProjectileSpeedControl:
                break;
            case EnchantEffect1.AddFrontProjectile:
                AddFrontBullet(skillName);
                
                break;
            case EnchantEffect1.AddSlashProjectile:
                AddSlashBullet(skillName);
                break;
        }
    }

    private void IncreaseSkillDamage(string skillName, int index)
    {
        string description = Datas.GameData.DTSkillEnchantData[index].description;
        float percent = GetPercentValue(description);

        float damage = _bulletPrefabDict[skillName].bulletDamage;
        damage += percent * damage * PERCENT_DIVISION;
        _bulletPrefabDict[skillName].bulletDamage = damage;
    }

    private int GetPercentValue(string description)
    {
        string[] strArr = description.Split();

        for (int i = 0; i < strArr.Length; i++)
        {
            int index = strArr[i].IndexOf('%');
            if (index != -1)
            {
                return int.Parse(strArr[i].Remove(index));
            }
        }

        return 0;
    }

    private void AddFirstBulletType(Bullet bullet)
    {
        if (_bulletPrefabDict.Count % 2 == 0)
        {
            _frontBullets.Add(bullet);
        }
        else
        {
            _frontBullets.Insert(0, bullet);
        }
        _bulletSpawnAreaCount++;
    }

    private void AddSlashBullet(string skillName)
    {
        _slashBullets.Insert(0, _bulletPrefabDict[skillName]);
        _slashBullets.Add(_bulletPrefabDict[skillName]);
        _bulletSpawnAreaCount += 2;
    }

    private void AddFrontBullet(string skillName)
    {
        int index = _frontBullets.IndexOf(_bulletPrefabDict[skillName]);
        if (index != -1)
        {
            _frontBullets.Insert(index, _bulletPrefabDict[skillName]);
        }
        else
        {
            _frontBullets.Add(_bulletPrefabDict[skillName]);
        }
        _bulletSpawnAreaCount++;
    }

    private void AddActiveBulletCallback(Bullet bullet)
    {
        _activeBullets.Add(bullet);
    }

    private void RemoveActiveBulletCallback(Bullet bullet)
    {
        _activeBullets.Remove(bullet);
    }

    IEnumerator FireContinuously()
    {
        // 무한 루프를 사용하여 지속적으로 총알을 발사
        while (true)
        {
            // 총알 인스턴스 생성
            if (_bulletPrefabDict.Count > 0)
            {
                _spawner.SpawnFrontBullets(_frontBullets);
                _spawner.SpawnSlashBullets(_slashBullets, Angle);
            }
            // 다음 발사까지 기다림 (초당 발사 횟수의 역수를 기다림 시간으로 사용)
            yield return new WaitForSeconds(1f / fireRate);
        }
    }

#if UNITY_EDITOR
    public void SetLowerBulletRotation(float value)
    {
        Angle = value;
    }
#endif
}