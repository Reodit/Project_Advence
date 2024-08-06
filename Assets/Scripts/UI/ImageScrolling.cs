using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
public class BackgroundSets
{
    public List<Sprite> backgrounds;
}

public class ImageScrolling : MonoBehaviour
{
    public static ImageScrolling Instance;
    public float scrollSpeed = 0.5f;
    public GameObject bgPrefab;
    public List<BackgroundSets> bgSprites;
    
    // TODO 업데이트가 아니라 스테이지를 파라미터로 던지는 함수 필요
    void Update()
    {
        if (!GameManager.instance.IsGamePaused)
        {
            if (SceneManager.GetActiveScene().name == "OutgameScene")
            {
                foreach (var e in scrollingImages[0])
                {
                    if (!e.gameObject.activeSelf)
                    {
                        e.gameObject.SetActive(true);
                    }
                    e.uvRect = new Rect(e.uvRect.position + Vector2.right * (scrollSpeed * Time.deltaTime), e.uvRect.size);
                }
            }

            else
            {
                foreach (var e in scrollingImages[GameManager.instance.currentStage - 1])
                {
                    if (!e.gameObject.activeSelf)
                    {
                        e.gameObject.SetActive(true);
                    }
                    e.uvRect = new Rect(e.uvRect.position + Vector2.right * (scrollSpeed * Time.deltaTime), e.uvRect.size);
                }
            }
            

        }
    }

    public Dictionary<int, List<RawImage>> scrollingImages;

    private void Awake()
    {
        Instance = this;
        scrollingImages = new Dictionary<int, List<RawImage>>();
        
        for (int i = 0; i < bgSprites.Count; i++)
        {
            List<RawImage> backgrounds = new List<RawImage>();
            
            for (int j = 0; j < bgSprites[i].backgrounds.Count; j++)
            {
                var rawImage = Instantiate(bgPrefab, this.transform).GetComponent<RawImage>();
                rawImage.texture = bgSprites[i].backgrounds[j].texture;
                rawImage.color = Color.white;
                backgrounds.Add(rawImage);
                rawImage.gameObject.SetActive(false);
            }
            
            scrollingImages.Add(i, backgrounds);
        }
    }
}
