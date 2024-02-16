using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // TODO need refactor this 
    [SerializeField] private int dataKey;
    public List<SpriteRenderer> spriteRenderers { get; private set; }
    public int CurrentHp { get; set; }
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

    public void HitPlayer(PlayerMove currentPlayer)
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

    // TODO Animation 관리 어떻게 ?

    private bool MoveTowardsAndCheckArrival(Vector3 currentPosition, Vector3 targetPosition, float moveSpeed, float threshold = 0.1f)
    {
        // Move towards the target position
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, step);

        // Check if the object is within the threshold distance of the target position
        bool hasArrived = Vector3.Distance(transform.position, targetPosition) <= threshold;

        return hasArrived;
    }

    private int GetNextWaypointIndex(int currentIndex, int waypointsCount, bool loop = true)
    {
        int nextIndex = currentIndex + 1;
        if (nextIndex >= waypointsCount)
        {
            nextIndex = loop ? 0 : waypointsCount - 1;
        }
        return nextIndex;
    }
}