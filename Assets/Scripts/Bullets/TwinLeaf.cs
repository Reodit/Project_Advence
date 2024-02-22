using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinLeaf : Bullet
{
    [SerializeField] private float secondBulletSpawnDelay = 0.1f;
    [SerializeField] private Bullet secondBullet;
    [field: SerializeField] private bool damagable = true;


    protected override void Start()
    {
        base.Start();

        StartCoroutine(SpawnSecondDelay());
    }

    private IEnumerator SpawnSecondDelay()
    {
        if (!damagable)
        {
            yield break;
        }

        yield return new WaitForSeconds(secondBulletSpawnDelay);

        TwinLeafSub bullet = ObjectPooler.Instance.Bullet.Instantiate(secondBullet, initPosition, Quaternion.identity).GetComponent<TwinLeafSub>();
        bullet.Init(BulletInfo);
    }

    public override void HitMonster(Monster monster)
    {
        base.HitMonster(monster);
    }
}
