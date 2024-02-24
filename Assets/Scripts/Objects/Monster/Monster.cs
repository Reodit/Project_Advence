using System;
using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IPooledObject
{
    // TODO need refactor this 
    [SerializeField] private int dataKey;
    public List<SpriteRenderer> spriteRenderers { get; private set; }
    public float CurrentHp { get; set; }
    [HideInInspector] public MonsterTable monsterData;

    public Animator Animator;

    [Header("Visual Effects")]
    public Image hpBar;
    public Color hitColor = Color.red;
    public float hitDuration = 0.1f;
    public float dieDelay;

    private string _monsterAttackCoolTimeID;
    public StateMachine<Monster> StateMachine { get; protected set; }

    protected virtual void Start()
    {
        monsterData = Datas.GameData.DTMonsterData[dataKey];
        CurrentHp = monsterData.MaxHP;
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        InitializeFsm();
    }
    
    protected virtual void InitializeFsm()
    {
    }

    public virtual void HitPlayer(PlayerMove currentPlayer)
    {
        EffectUtility.Instance.FlashHitColor(currentPlayer.spriteRenderers, currentPlayer.hitColor, currentPlayer.hitDuration);
        currentPlayer.currentHp -= monsterData.Attack;
        currentPlayer.Hpbar.fillAmount = (float)currentPlayer.currentHp / currentPlayer.characterData.maxHp;
    }
    
    public virtual void ProjectileHitPlayer(PlayerMove currentPlayer)
    {
        EffectUtility.Instance.FlashHitColor(currentPlayer.spriteRenderers, currentPlayer.hitColor, currentPlayer.hitDuration);
        currentPlayer.currentHp -= monsterData.Attack;
        currentPlayer.Hpbar.fillAmount = (float)currentPlayer.currentHp / currentPlayer.characterData.maxHp;
    }
    
    public void Die(float delay = 0f)
    {
        Destroy(this.gameObject, delay);
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.PlayerMove.currentExp += monsterData.EXP;
    }

    public virtual void RangeAttack()
    {
        
    }
    
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
    }

    public bool MoveToward(Vector2 targetPosition, float arrivalThreshold, float moveSpeed)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        if (distance <= arrivalThreshold)
        {
            return true;
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * (moveSpeed * Time.deltaTime));

        return false;
    }
    
    public bool MoveToward(Vector2 targetPosition, float arrivalThreshold, 
        float moveSpeed, float timeLimit, string moveTimerId)
    {
        if (!TimeManager.Instance.IsCoolTimeFinished(moveTimerId))
        {
            TimeManager.Instance.Use(moveTimerId);
            TimeManager.Instance.UpdateCoolTime(moveTimerId, timeLimit);
        }

        float distance = Vector2.Distance(transform.position, targetPosition);
        if (distance <= arrivalThreshold)
        {
            return true; 
        }

        if (TimeManager.Instance.IsCoolTimeFinished(moveTimerId))
        {
            return false; 
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * (moveSpeed * Time.deltaTime));

        return false; 
    }
    
    public bool MoveToward(Vector2 targetPosition, 
        float moveSpeed, float timeLimit, string moveTimerId)
    {
        if (!TimeManager.Instance.IsCoolTimeFinished(moveTimerId))
        {
            TimeManager.Instance.Use(moveTimerId);
            TimeManager.Instance.UpdateCoolTime(moveTimerId, timeLimit);
        }
        
        if (TimeManager.Instance.IsCoolTimeFinished(moveTimerId))
        {
            return true; 
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * (moveSpeed * Time.deltaTime));

        return false; 
    }

    public void OnObjectInstantiate()
    {
        throw new NotImplementedException();
    }

    public void OnObjectSpawn()
    {
        throw new NotImplementedException();
    }

    public void OnObjectReturn()
    {
        throw new NotImplementedException();
    }

    public void OnObjectDestroy()
    {
        throw new NotImplementedException();
    }
}
