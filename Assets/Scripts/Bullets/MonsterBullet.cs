using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : Bullet
{
    public override void RemoveBullet()
    {
        Destroy(this.gameObject);
    }
}
