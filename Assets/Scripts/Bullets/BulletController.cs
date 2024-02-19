using System;
using Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

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
    public bool isFamiliarBullet;
    
    private Dictionary<int, Bullet> _bulletPrefabDict = new Dictionary<int, Bullet>();
    
    private List<Bullet> _activeBullets = new List<Bullet>();

    private List<Bullet> _frontBullets = new List<Bullet>();
    private List<Bullet> _slashBullets = new List<Bullet>();
    private List<Bullet> _specialBullets = new List<Bullet>();

    private BulletSpawner _spawner;


    private Dictionary<int, BulletInfo> _bulletInfoDict = new Dictionary<int, BulletInfo>();
    public event Action OnFire;
    public void Start()
    {
        float bulletInterval = (float)bulletSpawnSpace / maxBulletSpawnArea;
        float bulletIntervalHalf = bulletInterval * 0.5f;

        spawnPoint = transform;

        _spawner = new BulletSpawner(
            spawnPoint, bulletInterval, bulletIntervalHalf,
            AddActiveBulletCallback, RemoveActiveBulletCallback);

        SkillManager.instance.OnAddSkill += AddSkillCallback;
        SkillManager.instance.OnAddEnchant += AddEnchantCallback;
        
        StartCoroutine(FireContinuously());
    }

    private void OnDestroy()
    {
        SkillManager.instance.OnAddSkill -= AddSkillCallback;
        SkillManager.instance.OnAddEnchant -= AddEnchantCallback;
    }

    public void AddSkillCallback(SkillTable skill)
    {
        Bullet bullet = Resources.Load<Bullet>(skill.prefabPath);
        if (!bullet)
        {
            return;
        }

        bullet.SetSkillIndex(skill.index);
        BulletInfo bulletInfo = new BulletInfo(skill.skillDamageRate, skill.skillSpeedRate, skill.range, skill.projectileSpeed);
        bullet.SetBulletInfo(bulletInfo);
        bullet.isFamiliarBullet = isFamiliarBullet;
        _bulletInfoDict.Add(skill.index, bulletInfo);

        _bulletPrefabDict.Add(skill.index, bullet);

        switch (skill.type)
        {
            case SkillType.Normal:
                AddFirstBulletType(bullet);
                break;
            case SkillType.Waterballoon:
                AddSpecialBullet(bullet);
                break;
            case SkillType.Outside:
                break;
            case SkillType.MeleeFamiliar:
                break;
            case SkillType.RangeFamiliar:
                break;
        }
    }

    private void AddSpecialBullet(Bullet bullet)
    {
        _specialBullets.Add(bullet);
    }

    public void AddEnchantCallback(int skillIndex, SkillEnchantTable enchant)
    {
        switch (enchant.enchantEffect1)
        {
            case Status.SkillDamageRate:
                IncreaseSkillDamage(skillIndex);
                break;
            case Status.AttackSpeedRate:
                //IncreaseSkillRate(skillIndex, enchant.index);
                break;
            case Status.ProjectileRange:
                //IncreaseSkillRange(skillIndex, enchant.index);
                break;
            case Status.ProjectileSpeed:
                //IncreaseBulletSpeed(skillIndex, enchant.index);
                break;
            case Status.AddFrontProjectile:
                IncreaseFrontBullet(skillIndex);
                break;
            case Status.AddSlashProjectile:
                IncreaseSlashBullet(skillIndex);
                break;
        }
    }

    private void IncreaseSkillDamage(int skillIndex)
    {
        BulletInfo bulletInfo = _bulletInfoDict[skillIndex];
        bulletInfo.SetDamage(SkillManager.instance.PlayerResultSkillDamage(skillIndex));

        _bulletInfoDict[skillIndex] = bulletInfo;

        ApplyActiveBullets(bulletInfo);
    }

    

    private void IncreaseSkillRate(int skillIndex)
    {
        BulletInfo bulletInfo = _bulletInfoDict[skillIndex];
        bulletInfo.SetSkillSpeedRate(SkillManager.instance.PlayerAttackSpeed(skillIndex));

        _bulletInfoDict[skillIndex] = bulletInfo;

        ApplyActiveBullets(bulletInfo);
    }


    private void IncreaseSkillRange(int skillIndex, int enchantIndex)
    {
        string description = GetDescrition(enchantIndex);
        int amount = ExcelUtility.GetPercentValue(description);
        float distance = _bulletPrefabDict[skillIndex].BulletInfo.MaxDistance;
        float afterDistance = _bulletInfoDict[skillIndex].MaxDistance + (distance * amount * Consts.PERCENT_DIVISION);

        BulletInfo bulletInfo = _bulletInfoDict[skillIndex];
        bulletInfo.SetMaxDistance(afterDistance);

        _bulletInfoDict[skillIndex] = bulletInfo;

        ApplyActiveBullets(bulletInfo);
    }

    private void IncreaseBulletSpeed(int skillIndex, int enchantIndex)
    {
        string description = GetDescrition(enchantIndex);
        int amount = ExcelUtility.GetPercentValue(description);
        float speed = _bulletPrefabDict[skillIndex].BulletInfo.Speed;
        float afterSpeed = _bulletInfoDict[skillIndex].Speed + (speed * amount * Consts.PERCENT_DIVISION);

        BulletInfo bulletInfo = _bulletInfoDict[skillIndex];
        bulletInfo.SetSpeed(afterSpeed);

        _bulletInfoDict[skillIndex] = bulletInfo;

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

    private void IncreaseSlashBullet(int skillIndex)
    {
        _slashBullets.Insert(0, _bulletPrefabDict[skillIndex]);
        _slashBullets.Add(_bulletPrefabDict[skillIndex]);
        _bulletSpawnAreaCount += 2;
    }

    private void IncreaseFrontBullet(int skillIndex)
    {
        int index = _frontBullets.IndexOf(_bulletPrefabDict[skillIndex]);
        if (index != -1)
        {
            _frontBullets.Insert(index, _bulletPrefabDict[skillIndex]);
        }
        else
        {
            _frontBullets.Add(_bulletPrefabDict[skillIndex]);
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
                _spawner.SpawnSpecialBullets(_specialBullets, _bulletInfoDict);
                OnFire?.Invoke();
            }
            yield return null;
        }

    }

#if UNITY_EDITOR
    public void SetLowerBulletRotation(float value)
    {
        Angle = value;
    }
#endif
}