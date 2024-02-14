using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterballoon : Bullet
{
    private float arriveTime = 1f;

    protected override IEnumerator DestroyAfterDelayCoroutine()
    {
        return base.DestroyAfterDelayCoroutine();
    }

    public override void HitMonster(Monster monster)
    {
        
    }
}
