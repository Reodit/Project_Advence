using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionBullet : Bullet
{
    [SerializeField] private ParticlePoolObject rangeParticle;
    [SerializeField] private float subParticleDestroyTime = 1f;
    private bool _isFirstTime = true;
    private int _collisionCount = 0;

    private ParticleSystem _impactParticle;


    protected override void Start()
    {
        base.Start();
        _impactParticle = ObjectPooler.Instance.Particle.Instantiate(rangeParticle, transform.position, Quaternion.identity)
            .GetComponent<ParticleSystem>();
    }

    protected override void OnDestroy()
    {
        DestroySubParticle();
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
            _impactParticle.Play();
            _impactParticle.GetComponent<AudioSource>().Play();
            Vector2 explosionPos = CalculateExplosionPos(monster);
            Collider2D[] colliders = GetCollidersFromCircle(explosionPos);
            CollisionManager.Instance.ExplodeFromCollider(this, colliders);
        }
        else
        {
            EffectUtility.Instance.FlashHitColor(monster.spriteRenderers, monster.hitColor, monster.hitDuration);
            ApplyDamage(monster);
            TriggerDestruction();
        }
    }

    private Collider2D[] GetCollidersFromCircle(Vector2 explosionPos)
    {
        float radius = _impactParticle.GetComponent<CircleCollider2D>().radius * _impactParticle.transform.localScale.x;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        return colliders;
    }

    private Vector2 CalculateExplosionPos(Monster monster)
    {
        Vector2 explosionPos = transform.position;
        explosionPos.x += monster.transform.position.x;
        explosionPos.x *= 0.5f;
        _impactParticle.transform.position = explosionPos;
        return explosionPos;
    }

    private void DestroySubParticle()
    {
        //if (_impactParticle != null)
        //    //ObjectPooler.Instance.Particle.return Destroy(_impactParticle.gameObject, subParticleDestroyTime);
    }
}
