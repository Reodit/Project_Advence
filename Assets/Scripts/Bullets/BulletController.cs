using System;
using Enums;
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
    
    private int _bulletSpawnAreaCount;

    private Dictionary<string, Bullet> _bulletPrefabDict = new Dictionary<string, Bullet>();
    
    private List<Bullet> _activeBullets = new List<Bullet>();

    private List<Bullet> _frontBullets = new List<Bullet>();
    private List<Bullet> _slashBullets = new List<Bullet>();

    private BulletSpawner _spawner;

    private const float PERCENT_DIVISION = 0.01f;

    private Dictionary<string, BulletInfo> _bulletInfoDict = new Dictionary<string, BulletInfo>();
    public event Action OnFire;
    public void Start()
    {
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

    // TODO 임시처리  ==> 구조화 다시 논의 필요
    public void AddSkillCallback(SkillTable skill)
    {
        Bullet bullet = Resources.Load<Bullet>(skill.prefabPath);
        if (!bullet)
        {
            return;
        }
        
        bullet.SetSkillName(skill.name);
        BulletInfo bulletInfo = new BulletInfo(bullet.BulletInfo.Damage, bullet.BulletInfo.MaxDistance, bullet.BulletInfo.Speed);

        _bulletInfoDict.Add(skill.name, bulletInfo);

        _bulletPrefabDict.Add(skill.name, bullet);

        switch (skill.type)
        {
            case SkillType.Normal:
                AddFirstBulletType(bullet);
                break;
            case SkillType.Waterballoon:
                break;
            case SkillType.Outside:
                break;
            case SkillType.Creature:
                break;
        }
    }

    public void AddEnchantCallback(string skillName, SkillEnchantTable enchant)
    {
        switch (enchant.enchantEffect1)
        {
            case EnchantEffect1.SkillDamageControl:
                IncreaseSkillDamage(skillName, enchant.index);
                break;
            case EnchantEffect1.AttackSpeedControl:
                IncreaseSkillRate(skillName, enchant.index);
                break;
            case EnchantEffect1.RangeControl:
                IncreaseSkillRange(skillName, enchant.index);
                break;
            case EnchantEffect1.ProjectileSpeedControl:
                IncreaseBulletSpeed(skillName, enchant.index);
                break;
            case EnchantEffect1.AddFrontProjectile:
                IncreaseFrontBullet(skillName);
                break;
            case EnchantEffect1.AddSlashProjectile:
                IncreaseSlashBullet(skillName);
                break;
        }
    }

    private void IncreaseSkillDamage(string skillName, int index)
    {
        string description = GetDescrition(index);
        float percent = ExcelUtility.GetPercentValue(description);

        float damage = _bulletInfoDict[skillName].Damage;
        float afterDamage = damage + damage * percent * PERCENT_DIVISION;

        BulletInfo bulletInfo = _bulletInfoDict[skillName];
        bulletInfo.SetDamage(afterDamage);

        _bulletInfoDict[skillName] = bulletInfo;

        ApplyActiveBullets(bulletInfo);
    }

    

    private void IncreaseSkillRate(string skillName, int index)
    {
        string description = GetDescrition(index);
        float percent = ExcelUtility.GetPercentValue(description);

        fireRate += fireRate * percent * PERCENT_DIVISION;
    }


    private void IncreaseSkillRange(string skillName, int index)
    {
        string description = GetDescrition(index);
        int amount = ExcelUtility.GetPercentValue(description);
        float afterDistance = _bulletInfoDict[skillName].MaxDistance;
        afterDistance += afterDistance * amount * PERCENT_DIVISION;

        BulletInfo bulletInfo = _bulletInfoDict[skillName];
        bulletInfo.SetMaxDistance(afterDistance);

        _bulletInfoDict[skillName] = bulletInfo;

        ApplyActiveBullets(bulletInfo);
    }

    private void IncreaseBulletSpeed(string skillName, int index)
    {
        string description = GetDescrition(index);
        int amount = ExcelUtility.GetPercentValue(description);
        float afterSpeed = _bulletInfoDict[skillName].Speed;
        afterSpeed += afterSpeed * amount * PERCENT_DIVISION;

        BulletInfo bulletInfo = _bulletInfoDict[skillName];
        bulletInfo.SetSpeed(afterSpeed);

        _bulletInfoDict[skillName] = bulletInfo;

        ApplyActiveBullets(bulletInfo);
    }

    private void ApplyActiveBullets(BulletInfo bulletInfo)
    {
        for (int i = 0; i < _activeBullets.Count; i++)
        {
            _activeBullets[i].SetBulletInfo(bulletInfo);
        }
    }

    private string GetDescrition(int index)
    {
        return Datas.GameData.DTSkillEnchantData[index].description;
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

    private void IncreaseSlashBullet(string skillName)
    {
        _slashBullets.Insert(0, _bulletPrefabDict[skillName]);
        _slashBullets.Add(_bulletPrefabDict[skillName]);
        _bulletSpawnAreaCount += 2;
    }

    private void IncreaseFrontBullet(string skillName)
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
                _spawner.SpawnFrontBullets(_frontBullets, _bulletInfoDict);
                _spawner.SpawnSlashBullets(_slashBullets, _bulletInfoDict, Angle);
                OnFire?.Invoke();
            }
            // 다음 발사까지 기다림 (초당 발사 횟수의 역수를 기다림 시간으로 사용)
            yield return new WaitForSeconds(1f / fireRate);
        }
    }
    
    IEnumerator FamiliarSpawnCo(Familiar familiar)
    {
        Instantiate(familiar, GameManager.Instance.PlayerMove.transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(familiar.spawnCoolTime);
    }

#if UNITY_EDITOR
    public void SetLowerBulletRotation(float value)
    {
        Angle = value;
    }
#endif
}