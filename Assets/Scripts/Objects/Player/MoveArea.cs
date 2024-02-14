using UnityEngine;
 
public class MoveArea : MonoBehaviour
{
    public static MoveArea Instance;
    public Transform square; // Square 게임 오브젝트를 에디터에서 할당
    private Vector2 squareMin; // Square의 최소 X와 Y 위치
    private Vector2 squareMax; // Square의 최대 X와 Y 위치

    void Start()
    {
        square = this.transform;
        CalculateSquareBounds();
    }

    // Square 영역의 경계 계산
    void CalculateSquareBounds()
    {
        // Square 게임 오브젝트의 중심 위치
        Vector3 squareCenter = square.position;
        // Square 게임 오브젝트의 스케일
        Vector3 squareScale = square.localScale;

        // Square 영역의 최소와 최대 위치 계산
        squareMin = squareCenter - (squareScale * 0.5f);
        squareMax = squareCenter + (squareScale * 0.5f);
    }

    // 플레이어의 위치를 Square 영역 안으로 제한
    public Vector3 ConstrainPosition(Vector3 proposedPosition)
    {
        proposedPosition.x = Mathf.Clamp(proposedPosition.x, squareMin.x, squareMax.x);
        proposedPosition.y = Mathf.Clamp(proposedPosition.y, squareMin.y, squareMax.y);
        return proposedPosition;
    }
}