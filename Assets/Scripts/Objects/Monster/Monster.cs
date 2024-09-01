using System;
using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // TODO need refactor this 
    [field: SerializeField] public int DataKey { get; private set; }
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

    public delegate void MonsterKilledHandler(Monster monster);
    public event MonsterKilledHandler OnDie;
    
    protected virtual void Start()
    {
        monsterData = Datas.GameData.DTMonsterData[DataKey];
        CurrentHp = monsterData.MaxHP;
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        OnDie += StageManager.instance.OnMonsterDie;
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
        currentPlayer.currentHp -= monsterData.RangeAttack;
        currentPlayer.Hpbar.fillAmount = (float)currentPlayer.currentHp / currentPlayer.characterData.maxHp;
    }
    
    public void Die(float delay = 0f)
    { 
        // TODO add Delay logic 지연 사망 기능 필요한가;;?
        // TODO 보스몬스터 클리어 시 무한 DIE 함수 불리는거 확인 필요
        this.gameObject.SetActive(false);
        OnDie?.Invoke(this);
        Destroy(this.gameObject);
    }

    // TODO 몬스터는 풀링하지 않는 걸로..
    protected virtual void OnDestroy()
    {
        // ObjectPoolManager.instance.ReturnToPool("Monster", monsterData.PrefabPath, this.gameObject);
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
}
