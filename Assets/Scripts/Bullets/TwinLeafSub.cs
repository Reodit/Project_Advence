using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinLeafSub : Bullet
{
    protected override void Start()
    {
        InitPosition = transform.position;
    }

    public void Init(BulletInfo bulletInfo)
    {
        BulletInfo = bulletInfo;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            Destroy(gameObject);
        }
    }
}
