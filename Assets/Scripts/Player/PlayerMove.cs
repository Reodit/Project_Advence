using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어의 이동 속도
    public MoveArea moveArea; // MoveArea 컴포넌트를 에디터에서 할당
    public int currentHp;
    public int playerMaxHp;
    public Image Hpbar;
    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;

        // MoveArea 컴포넌트를 사용하여 새 위치가 이동 영역 내에 있는지 확인
        newPosition = moveArea.ConstrainPosition(newPosition);

        transform.position = newPosition;
    }
    
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        currentHp = playerMaxHp;
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
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
