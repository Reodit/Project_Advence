using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Waterballoon : Bullet
{
    [SerializeField] private ExplosionTrigger rangeParticlePrefab;

    private ExplosionTrigger _rangeParticleTrigger;

    private float _distanceY;
    private float _startPosY;

    private bool _isMovable = false;

    [field: SerializeField] public Vector2 Scale { get; private set; }

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
        _rangeParticleTrigger = Instantiate(rangeParticlePrefab, startPos, Quaternion.identity);

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
            pixelArsenalProjectileScript.projectileParticle.gameObject.SetActive(false);
            CollisionManager.Instance.ExplodeFromCollider(this, _rangeParticleTrigger.Colliders);
            Destroy(_rangeParticleTrigger);
            // range에 Trigger 붙이기
            pixelArsenalProjectileScript.OnCol();
        }
    }

    public override void HitMonster(Monster monster)
    {
        EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);

        ApplyDamage(monster);
        TriggerDestruction();
    }

    public void SetScale(Vector2 scale)
    {
        Scale = scale;
        float scaleRatio = scale.x / Scale.x;
        myTrans.localScale = scale;
        rangeParticlePrefab.transform.localScale *= scaleRatio;
    }
}
