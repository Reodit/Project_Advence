using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSBullet : Bullet
{
    [SerializeField] private GameObject subBullet;

    [SerializeField] private float intervalPerBullet = 3f;

    [SerializeField] private float initPosX = -9f;

    protected override void Start()
    {
        InitPosition = transform.position;
        InitPosition.x = initPosX;
        transform.position = InitPosition;
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
        Vector2 afterPos = InitPosition;
        subBullet.SetActive(true);
        Bullet bullet = subBullet.GetComponent<Bullet>();
        afterPos.y += interval;
        bullet.Init(BulletInfo, null, SkillIndex);
    }
}
