using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Waterballoon : Bullet
{
    [SerializeField] private GameObject rangeParticlePrefab;

    private GameObject _rangeParticle;

    private float _distanceY;
    private float _startPosY;

    private bool _isMovable = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    private void Init()
    {
        Vector2 startPos = myTrans.position;
        _startPosY = startPos.y;
        startPos.x += BulletInfo.MaxDistance;
        _rangeParticle = Instantiate(rangeParticlePrefab, startPos, Quaternion.identity);

        _distanceY = Screen.width * Consts.PERCENT_DIVISION;
        startPos.y += _distanceY;
        myTrans.position = startPos;
    }

    protected override void Update()
    {
        CheckArrived();
        Move();
    }

    private void Move()
    {
        if (!_isMovable)
            return;

        Vector2 nextPos = Vector2.down * _distanceY * Time.deltaTime * BulletInfo.Speed;
        myTrans.position += new Vector3(nextPos.x, nextPos.y, 0f);
    }

    private void CheckArrived()
    {
        _isMovable = _startPosY < myTrans.position.y;

        if (!_isMovable)
        {
            pixelArsenalProjectileScript.projectileParticle.SetActive(false);
            CollisionManager.Instance.ExplodeFromCapsule2D(this, _rangeParticle.GetComponent<ExplosionTrigger>().Colliders);
            Destroy(_rangeParticle);
            // range에 Trigger 붙이기
            pixelArsenalProjectileScript.OnCol();
        }
    }

    public override void HitMonster(Monster monster)
    {
        EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);

        // TODO 몬스터 데미지 계산 통일필요
        monster.CurrentHp -= (int)BulletInfo.Damage;
        monster.hpBar.fillAmount = (float)monster.CurrentHp / monster.monsterData.MaxHP;
        TriggerDestruction();
    }
}
