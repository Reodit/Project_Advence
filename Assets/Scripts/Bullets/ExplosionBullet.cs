using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionBullet : Bullet
{
    [SerializeField] private float subParticleDestroyTime = 1f;
    private bool _isFirstTime = true;
    private int _collisionCount = 0;
    [SerializeField] private ParticleSystem impactParticle;


    protected override void Start()
    {
        base.Start(); 
        impactParticle.gameObject.SetActive(true);
    }

    private void OnDisable()
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
            impactParticle.Play();
            impactParticle.GetComponent<AudioSource>().Play();
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
        float radius = impactParticle.GetComponent<CircleCollider2D>().radius * impactParticle.transform.localScale.x;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        return colliders;
    }

    private Vector2 CalculateExplosionPos(Monster monster)
    {
        Vector2 explosionPos = transform.position;
        explosionPos.x += monster.transform.position.x;
        explosionPos.x *= 0.5f;
        impactParticle.transform.position = explosionPos;
        return explosionPos;
    }

    private void DestroySubParticle()
    {
        if (impactParticle != null)
        {
            impactParticle.gameObject.SetActive(false);
        }
    }
}
