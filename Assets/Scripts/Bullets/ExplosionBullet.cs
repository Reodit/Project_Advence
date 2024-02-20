using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : Bullet
{
    [SerializeField] private ParticleSystem rangeParticle;
    private bool _isFirstTime = true;
    private int _collisionCount = 0;

    private ParticleSystem _impactParticle;

    protected override void Start()
    {
        base.Start();
        _impactParticle = Instantiate(rangeParticle, transform.position, Quaternion.identity);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionManager.Instance.HandleCollision(gameObject, collision.gameObject);
    }

    public override void HitMonster(Monster monster)
    {
        if (_isFirstTime)
        {
            _isFirstTime = false;
            _impactParticle.transform.position = transform.position;
            _impactParticle.Play();
            List<Collider2D> colliders = _impactParticle.GetComponent<ExplosionTrigger>().Colliders;
            _collisionCount = colliders.Count;
            CollisionManager.Instance.ExplodeFromCollider(this, colliders);
        }
        else
        {
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);
            _collisionCount--;
            ApplyDamage(monster);
        }

        if (_collisionCount == 0)
        {
            Destroy(_impactParticle.gameObject, 5f);
            TriggerDestruction();
        }
    }
}
