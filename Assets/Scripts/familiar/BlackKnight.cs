using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlackKnight : Familiar
{
    protected int Hp;
    protected Vector3 currentPos;

    protected override void Start()
    {
        Hp = 0;
        base.Start();
    }

    // hit logic
    protected override bool CheckDestroyCondition()
    {
        bool needDestroy = base.CheckDestroyCondition();

        if (Hp <= 0)
        {
            needDestroy = true;
        }

        return needDestroy;
    }


    // atk
}
