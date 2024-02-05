using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private float triggerCooldown = 0.5f;

    private PlayerMove _player;

    private List<SpriteRenderer> _spriteRenderers = new ();
    public int monsterMaxHp;
    public int CurrentHp { get; set; }
    public float dieDelay;
    public int rewardExp;
    public Image Hpbar;
    public int attackDamage;
    
    public Color hitColor = Color.red;
    public float hitDuration = 0.2f;

    private string _monsterAttackCoolTimeID;
    
    void Start()
    {
        _player = GameManager.Instance.PlayerMove;
        CurrentHp = monsterMaxHp;
        _spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        attackDamage = 5;
        
        _monsterAttackCoolTimeID = "MonsterAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterAttackCoolTimeID, triggerCooldown);
    }

    private void Update()
    {
        if (CurrentHp <= 0)
        {
            Die(dieDelay);
        }
    }

    public void HitPlayer()
    {
        _player.isHit = true;
        _player.currentHp -= attackDamage;
        _player.Hpbar.fillAmount = (float)_player.currentHp / _player.characterData.maxHp;
    }

    public void Die(float delay = 0f)
    {
        Destroy(this.gameObject, delay);
    }

    public void OnDestroy()
    {
        GameManager.Instance.PlayerMove.currentExp += rewardExp;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (TimeManager.Instance.IsCoolTimeFinished(_monsterAttackCoolTimeID))
        {
            // 쿨타임이 완료되었으므로 공격 처리
            CollisionManager.Instance.HandleCollision(this.gameObject, collision.gameObject);

            // 쿨타임 재설정
            TimeManager.Instance.Use(_monsterAttackCoolTimeID);
        }
    }

    public IEnumerator FlashHitColor()
    {
        List<Color> originalColors = new List<Color>();

        foreach (var spriteRenderer in _spriteRenderers)
        {
            originalColors.Add(spriteRenderer.color);
            spriteRenderer.color = hitColor;
        }

        yield return new WaitForSeconds(hitDuration);

        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            _spriteRenderers[i].color = originalColors[i];
        }
    }
}
