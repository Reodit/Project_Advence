using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinLeafSub : Bullet
{
    protected override void Start()
    {
        initPosition = transform.position;
    }

    protected override void OnDestroy()
    {
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
