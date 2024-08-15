using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TwinLeaf : Bullet
{
    [SerializeField] private float secondBulletSpawnDelay = 0.1f;
    [SerializeField] private GameObject secondBullet;
    private bool damageAble = true;


    protected override void Start()
    {
        base.Start();

        StartCoroutine(SpawnSecondDelay());
    }

    private IEnumerator SpawnSecondDelay()
    {
        if (!damageAble)
        {
            yield break;
        }

        yield return new WaitForSeconds(secondBulletSpawnDelay);
        
        secondBullet.gameObject.SetActive(true);
        TwinLeafSub bullet = secondBullet.GetComponent<TwinLeafSub>();
        bullet.Init(BulletInfo);
    }

    public override void HitMonster(Monster monster)
    {
        base.HitMonster(monster);
    }
}
