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

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    public int monsterMaxHp;
    public int currentHp;
    public float dieDelay;
    public int rewardExp;
    public Image Hpbar;
    void Start()
    {
        currentHp = monsterMaxHp;
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    private void Update()
    {
        if (currentHp <= 0)
        {
            Die();
        }
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

    void OnTriggerStay(Collider other)
    {
        // 현재 시간이 마지막 트리거 호출 시간 + 쿨다운 시간보다 큰 경우에만 로직 실행
        if (Time.time > lastTriggerTime + triggerCooldown && other.gameObject.tag == "Player")
        {
            var player = other.GetComponent<PlayerMove>();
            player.isHit = true;
            player.currentHp -= 3;
            player.Hpbar.fillAmount = (float) player.currentHp / player.playerMaxHp;
            lastTriggerTime = Time.time;
        }
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
