using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + Vector2.right * scrollSpeed * Time.deltaTime, rawImage.uvRect.size);

        // 필요한 경우, uvRect를 재설정하여 무한 스크롤링 효과를 만듭니다.
    }
}
