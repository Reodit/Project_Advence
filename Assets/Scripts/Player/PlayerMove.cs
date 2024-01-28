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
    public Dictionary<int, int> needExpNextLvl;
    public int currentExp;
    public int currentLvl;
    public int maxLvl;
    public bool isHit = false;
    private Coroutine flashCoroutine = null; // 코루틴의 참조를 저장할 변수

    void Update()
    {
        Move();
        LevelUp();
        if (isHit)
        {
            Hit();
        }
    }

    public void LevelUp()
    {
        if (currentLvl >= maxLvl)
        {
            currentExp = needExpNextLvl[maxLvl];
            currentLvl = maxLvl;
            return;
        }
        
        if (needExpNextLvl[currentLvl] <= currentExp)
        {
            currentExp -= needExpNextLvl[currentLvl];
            currentLvl++;
        }

    }
    
    public void Awake()
    {

    }

    public void Init()
    {
        GameManager.Instance.PlayerMove = this;
        needExpNextLvl = new Dictionary<int, int>();
        needExpNextLvl.Add(0, 0);
        needExpNextLvl.Add(1, 30);
        needExpNextLvl.Add(2, 60);
        needExpNextLvl.Add(3, 90);
        needExpNextLvl.Add(4, 120);
        currentLvl = 0;
        currentExp = 0;
        maxLvl = needExpNextLvl.Count - 1;
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
        isHit = false;
        if (flashCoroutine == null) // 코루틴이 실행 중이 아니라면 코루틴 실행
        {
            flashCoroutine = StartCoroutine(FlashHitColor());
        }    
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
        
        flashCoroutine = null; // 코루틴이 끝났음을 표시
    }
}
