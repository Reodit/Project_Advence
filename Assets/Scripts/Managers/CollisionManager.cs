using MathNet.Numerics.Statistics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        } 
        
        else 
        {
            Destroy(gameObject);
        }
    }
    
    public void HandleCollision(GameObject collider, GameObject collidee)
    {
        if (collider.CompareTag("PlayerProjectile") && collidee.CompareTag("Monster"))
        {
            if (collider.TryGetComponent(out Bullet bullet) && 
                collidee.TryGetComponent(out Monster monster))
            {
                if (monster.CurrentHp > 0)
                {
                    bullet.HitMonster(monster);   
                }
            }
        }
        
        else if (collider.CompareTag("MonsterProjectile") && collidee.CompareTag("Player"))
        {
            if (collider.TryGetComponent(out Bullet bullet) && 
                collidee.TryGetComponent(out PlayerMove player))
            {
                bullet.transform.parent.
                    transform.parent.GetComponent<Monster>().HitPlayer(player);
            }
        }

        else if (collider.CompareTag("Monster") && collidee.CompareTag("Player"))
        {
            if (collider.TryGetComponent(out Monster monster) && 
                collidee.TryGetComponent(out PlayerMove player))
            {
                monster.HitPlayer(player);
            }
        }
        
        else if (collider.CompareTag("Familiar") && collidee.CompareTag("Monster"))
        {
            if (collider.TryGetComponent(out Familiar familiar) && 
                collidee.TryGetComponent(out Monster monster))
            {
                familiar.HitMonster(monster);
            }
        }
        
        /*else if (collider.CompareTag("") && collidee.CompareTag(""))
        {
            
        }*/
    }

    public void ExplodeFromCollider(Bullet bullet, List<Collider2D> colliders)
    {
        foreach (var col in colliders)
        {
            if (bullet.tag == "PlayerProjectile")
            {
                if (col.TryGetComponent(out Monster monster))
                {
                    bullet.HitMonster(monster);
                }
            }
        }
    }

    public void ExplodeFromCollider(Bullet bullet, Collider2D[] colliders)
    {
        foreach (var col in colliders)
        {
            if (bullet.tag == "PlayerProjectile")
            {
                if (col.TryGetComponent(out Monster monster))
                {
                    bullet.HitMonster(monster);
                }
            }
        }
    }

}
