using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSBullet : Bullet
{
    [SerializeField] private Bullet subBullet;

    [SerializeField] private float intervalPerBullet = 3f;

    [SerializeField] private float initPosX = -9f;

    protected override void Start()
    {
        initPosition = transform.position;
        initPosition.x = initPosX;
        transform.position = initPosition;
        base.Start();
        SpawnSubBullets();
    }

    private void SpawnSubBullets()
    {
        SpawnSubBullet(intervalPerBullet);
        SpawnSubBullet(-intervalPerBullet);
    }

    private void SpawnSubBullet(float interval)
    {
        Vector2 afterPos = initPosition;
        afterPos.y += interval;
        Bullet bullet = ObjectPoolManager.instance.SpawnFromPool("Bullet", subBullet.BulletInfo.bulletPrefabPath, subBullet.transform.position,
            subBullet.transform.rotation, subBullet.transform.localScale).GetComponent<Bullet>();
        bullet.Init(BulletInfo, null, SkillIndex);
    }
}
