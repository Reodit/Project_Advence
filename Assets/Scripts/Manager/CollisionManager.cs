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
    
    // Collision 처리모음 (FSM, Animation 포함)
    public void HandleCollision(GameObject collider, GameObject collidee) 
    {
        // 총알 --> 몬스터 (hit)
        if (collider.CompareTag("PlayerBullet") && collidee.CompareTag("Monster"))
        {
            if (collider.TryGetComponent(out Bullet bullet) &&
                collidee.TryGetComponent(out Monster monster))
            {
                bullet.HitMonster(monster);
            }
        }
        
        // 몬스터 --> 플레이어 (hit)
        else if (collider.CompareTag("Monster") && collidee.CompareTag("Player"))
        {
            if (collider.TryGetComponent(out Monster monster))
            {
                monster.HitPlayer();
            }
        }
        
        // 소환수(근접) (hit) --> 몬스터 (hit)
        else if (collider.CompareTag("Familiar") && collidee.CompareTag("Monster"))
        {
            
        }
        
        // 몬스터(원거리) --> 플레이어 (hit)
        /*else if (collider.CompareTag("") && collidee.CompareTag(""))
        {
            
        }*/
    }

}
