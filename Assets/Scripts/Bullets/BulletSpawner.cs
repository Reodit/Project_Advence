using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner
{
    private Transform _spawnPoint;

    private float _bulletInterval;
    private float _bulletIntervalHalf;

    private float _topForntBulletPosY;
    private float _bottomFrontBulletPosY;

    private Action<Bullet> _onAddBullet;
    private Action<Bullet> _onRemoveBullet;

    private Dictionary<Bullet, float> _spawnTimerDict = new Dictionary<Bullet, float>();

    private HashSet<Bullet> _spawnableBullets = new HashSet<Bullet>();

    public BulletSpawner(Transform spawnPoint, float bulletInterval, float bulletIntervalHalf, Action<Bullet> onAddBullet, Action<Bullet> onRemoveBullet)
    {
        _spawnPoint = spawnPoint;
        _bulletInterval = bulletInterval;
        _bulletIntervalHalf = bulletIntervalHalf;
        _onAddBullet = onAddBullet;
        _onRemoveBullet = onRemoveBullet;
    }

    public void SpawnFrontBullets(List<Bullet> frontBullets, Dictionary<int, BulletInfo> bulletInfoDict)
    {
        int frontBulletCount = frontBullets.Count;
        int halfCount = frontBulletCount / 2;
        Vector2 spawnPos = Vector2.zero;

        Bullet bullet;

        if (frontBulletCount % 2 == 0)
        {
            for (int i = 0; i < frontBulletCount; i++)
            {
                bullet = frontBullets[i];
                CheckSpawnable(bullet);

                if (!_spawnableBullets.Contains(bullet))
                    continue;

                spawnPos = _spawnPoint.position;
                spawnPos.y += (i - halfCount + 1) * _bulletInterval - _bulletIntervalHalf;
                SpawnBullet(bullet, spawnPos, bulletInfoDict[bullet.SkillIndex]);
                SetSideBulletPosY(frontBulletCount, i, spawnPos);
            }
        }
        else
        {
            for (int i = 0; i < frontBulletCount; i++)
            {
                bullet = frontBullets[i];
                CheckSpawnable(bullet);

                if (!_spawnableBullets.Contains(bullet))
                    continue;

                spawnPos = _spawnPoint.position;
                spawnPos.y += (i - halfCount) * _bulletInterval;
                SpawnBullet(frontBullets[i], spawnPos, bulletInfoDict[bullet.SkillIndex]);
                SetSideBulletPosY(frontBulletCount, i, spawnPos);
            }
        }

        if (frontBullets.Count == 1)
        {
            _topForntBulletPosY = spawnPos.y;
        }
    }

    public void SpawnSlashBullets(List<Bullet> slashBullets, Dictionary<int, BulletInfo> bulletInfoDict, float angle)
    {
        int slashBulletCount = slashBullets.Count;
        int halfCount = slashBulletCount / 2;

        for (int i = 0; i < slashBullets.Count; i++)
        {
            Bullet bullet = slashBullets[i];

            CheckSpawnable(bullet);

            if (!_spawnableBullets.Contains(bullet))
                continue;

            SpawnSlashBullet(bulletInfoDict, bullet, angle, halfCount, i);
        }

        if (_spawnableBullets.Count > 0)
            _spawnableBullets.Clear();
    }

    private void SpawnSlashBullet(Dictionary<int, BulletInfo> bulletInfoDict, Bullet bullet, float angle, int halfCount, int i)
    {
        Vector2 spawnPos = _spawnPoint.position;
        Quaternion quat = Quaternion.identity;
        if (i < halfCount)
        {
            spawnPos.y = _topForntBulletPosY + (halfCount - i) * _bulletInterval;
            quat = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            spawnPos.y = _bottomFrontBulletPosY - (i - halfCount + 1) * _bulletInterval;
            quat = Quaternion.Euler(0, 0, -angle);
        }

        SpawnBullet(bullet, spawnPos, quat, bulletInfoDict[bullet.SkillIndex]);
    }

    public void SpawnSpecialBullets(List<Bullet> specialBullets, Dictionary<int, BulletInfo> bulletInfoDict)
    {
        Vector2 spawnPos = _spawnPoint.position;
        for (int i = 0; i < specialBullets.Count; i++)
        {
            Bullet bullet = specialBullets[i];

            CheckSpawnable(bullet);

            if (!_spawnableBullets.Contains(bullet))
                continue;

            SpawnBullet(bullet, spawnPos, bulletInfoDict[bullet.SkillIndex]);
        }

        if (_spawnableBullets.Count > 0)
            _spawnableBullets.Clear();
    }

    private bool CheckSpawnable(Bullet bullet)
    {
        bool isSpawnable = IsSpawnable(bullet, SkillManager.instance.PlayerAttackSpeed(bullet.SkillIndex));

        if (isSpawnable)
            _spawnableBullets.Add(bullet);

        return isSpawnable;
    }

    private bool IsSpawnable(Bullet bullet, float bulletSpeedRate)
    {
        if (!_spawnTimerDict.ContainsKey(bullet))
        {
            _spawnTimerDict.Add(bullet, 0f);
        }

        float currentTime = Time.time;
        float lastSpawnTime = _spawnTimerDict[bullet];

        bool isSpawnable = currentTime - lastSpawnTime >= 1 / bulletSpeedRate;

        if (isSpawnable)
            _spawnTimerDict[bullet] = currentTime;

        return isSpawnable;
    }

    private void SetSideBulletPosY(int frontBulletCount, int i, Vector2 spawnPos)
    {
        if (i == 0)
        {
            _bottomFrontBulletPosY = spawnPos.y;
        }
        else if (i == frontBulletCount - 1)
        {
            _topForntBulletPosY = spawnPos.y;
        }
    }

    void SpawnBullet(Bullet bulletPrefab, Vector2 pos, BulletInfo bulletInfo)
    {
        // 프리팹이 제대로 불러와졌는지 확인합니다.
        if (bulletPrefab != null)
        {
            Bullet bullet = UnityEngine.Object.Instantiate(bulletPrefab, pos, Quaternion.identity);
            bullet.Init(bulletInfo, RemoveBullet, bulletPrefab.SkillIndex);
            _onAddBullet.Invoke(bullet);
        }
    }

    void SpawnBullet(Bullet bulletPrefab, Vector2 pos, Quaternion quaternion, BulletInfo bulletInfo)
    {
        // 프리팹이 제대로 불러와졌는지 확인합니다.
        if (bulletPrefab != null)
        {
            Bullet bullet = UnityEngine.Object.Instantiate(bulletPrefab, pos, quaternion);
            bullet.Init(bulletInfo, RemoveBullet, bulletPrefab.SkillIndex);
            _onAddBullet.Invoke(bullet);
        }
    }

    private void RemoveBullet(Bullet bullet)
    {
        _onRemoveBullet.Invoke(bullet);
    }

}