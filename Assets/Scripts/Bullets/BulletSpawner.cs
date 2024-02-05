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

    public BulletSpawner(Transform spawnPoint, float bulletInterval, float bulletIntervalHalf, Action<Bullet> onAddBullet, Action<Bullet> onRemoveBullet)
    {
        _spawnPoint = spawnPoint;
        _bulletInterval = bulletInterval;
        _bulletIntervalHalf = bulletIntervalHalf;
        _onAddBullet = onAddBullet;
        _onRemoveBullet = onRemoveBullet;
    }

    public void SpawnSlashBullets(List<Bullet> slashBullets, float angle)
    {
        int slashBulletCount = slashBullets.Count;
        int halfCount = slashBulletCount / 2;

        for (int i = 0; i < slashBullets.Count; i++)
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

            SpawnBullet(slashBullets[i], spawnPos, quat);
        }
    }

    public void SpawnFrontBullets(List<Bullet> frontBullets)
    {
        int frontBulletCount = frontBullets.Count;
        int halfCount = frontBulletCount / 2;
        Vector2 spawnPos = Vector2.zero;

        if (frontBulletCount % 2 == 0)
        {
            for (int i = 0; i < frontBulletCount; i++)
            {
                spawnPos = _spawnPoint.position;
                spawnPos.y += (i - halfCount + 1) * _bulletInterval - _bulletIntervalHalf;
                SpawnBullet(frontBullets[i], spawnPos);
                SetSideBulletPosY(frontBulletCount, i, spawnPos);
            }
        }
        else
        {
            for (int i = 0; i < frontBulletCount; i++)
            {
                spawnPos = _spawnPoint.position;
                spawnPos.y += (i - halfCount) * _bulletInterval;
                SpawnBullet(frontBullets[i], spawnPos);
                SetSideBulletPosY(frontBulletCount, i, spawnPos);
            }
        }

        if (frontBullets.Count == 1)
        {
            _topForntBulletPosY = spawnPos.y;
        }
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

    void SpawnBullet(Bullet bulletPrefab, Vector2 pos)
    {
        // 프리팹이 제대로 불러와졌는지 확인합니다.
        if (bulletPrefab != null)
        {
            Bullet bullet = UnityEngine.Object.Instantiate(bulletPrefab, pos, Quaternion.identity);
            bullet.Init(RemoveBullet);
            _onAddBullet.Invoke(bullet);
        }
    }

    void SpawnBullet(Bullet bulletPrefab, Vector2 pos, Quaternion quaternion)
    {
        // 프리팹이 제대로 불러와졌는지 확인합니다.
        if (bulletPrefab != null)
        {
            Bullet bullet = UnityEngine.Object.Instantiate(bulletPrefab, pos, quaternion);
            bullet.Init(RemoveBullet);
            _onAddBullet.Invoke(bullet);
        }
    }

    private void RemoveBullet(Bullet bullet)
    {
        _onRemoveBullet.Invoke(bullet);
    }
}