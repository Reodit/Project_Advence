using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunction : MonoBehaviour
{
    public Monster monster;
    public void TurnOffAttack()
    {
        
    }

    public void MagicAttack()
    {
        
    }

    public void RangedAttack()
    {
        if (monster is RangeMonster range)
        {
            range.InstantiateProjectile();
        }
        else if (monster is S1P1BossMonster boss)
        {
            boss.InstantiateProjectile();
        }
    }
}
