using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어의 이동 속도
    public MoveArea moveArea; // MoveArea 컴포넌트를 에디터에서 할당

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
}
