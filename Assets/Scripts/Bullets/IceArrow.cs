using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : Bullet
{
    [SerializeField] private int penetrateCount = 1;

    public override void HitMonster(Monster monster)
    {
        if (penetrateCount > 0)
        {
            penetrateCount--;
        }
        else
        {
            pixelArsenalProjectileScript.OnCol();
            TriggerDestruction();
        }

        EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);
        ApplyDamage(monster);
    }
}
