using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private float triggerCooldown = 0.5f; // 트리거 호출 간의 쿨다운 시간 (0.5초)
    [SerializeField] private float lastTriggerTime = -0.5f; // 마지막으로 트리거 함수가 호출된 시간
    
    
    private CircleCollider2D _collider;
    private Vector2 _centerPos;
    private float _radius;
    private PlayerMove _player;


    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    public int monsterMaxHp;
    public int currentHp;
    public float dieDelay;
    public int rewardExp;
    public Image Hpbar;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        
    }

    void Start()
    {
        _player = GameManager.Instance.PlayerMove;
        currentHp = monsterMaxHp;
        _centerPos = _collider.bounds.center;
        _radius = _collider.radius;
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    private void Update()
    {
        if (currentHp <= 0)
        {
            Die();
        }

        if (CheckDistance() && CheckPreviousHit())
        {
            ApplyHitInfo();
        }
    }

    private void ApplyHitInfo()
    {
        _player.isHit = true;
        _player.currentHp -= 3;
        _player.Hpbar.fillAmount = (float)_player.currentHp / _player.characterData.maxHp;
        lastTriggerTime = Time.time;
    }

    private bool CheckDistance()
    {
        float distance = Vector2.Distance(_player.transform.position, transform.position);
        return distance <= _radius;
    }

    public int CurrentHp()
    {
        return currentHp;
    }
    
    public void Die()
    {
        Destroy(this.gameObject ,dieDelay);
    }

    public void OnDestroy()
    {
        GameManager.Instance.PlayerMove.currentExp += rewardExp;
    }

    private bool CheckPreviousHit()
    {
        return Time.time > lastTriggerTime + triggerCooldown;
    }

    public Color hitColor = Color.red; // 피격 시 적용할 색상
    public float hitDuration = 0.2f; // 피격 색상이 지속되는 시간

    // 피격 함수
    public void Hit()
    {
        StartCoroutine(FlashHitColor());
    }

    // 피격 시 색상 변경 코루틴
    IEnumerator FlashHitColor()
    {
        // 원래 색상을 저장할 리스트
        List<Color> originalColors = new List<Color>();

        // 각 스프라이트 렌더러의 원래 색상을 저장하고 피격 색상으로 변경
        foreach (var spriteRenderer in spriteRenderers)
        {
            originalColors.Add(spriteRenderer.color);
            spriteRenderer.color = hitColor;
        }

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(hitDuration);

        // 각 스프라이트 렌더러의 색상을 원래 색상으로 복원
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }
    }
}
