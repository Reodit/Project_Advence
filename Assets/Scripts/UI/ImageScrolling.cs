using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
[Serializable]
public class BackgroundSets
{
    public List<Sprite> backgrounds;
}

public class ImageScrolling : MonoBehaviour
{
    public static ImageScrolling Instace;
    public float scrollSpeed = 0.5f;
    public GameObject bgPrefab;
    public List<BackgroundSets> bgSprites;
    
    void Update()
    {
        if (!GameManager.Instance.isGamePause)
        {
            foreach (var e in scrollingImages[GameManager.Instance.currentStage - 1])
            {
                if (!e.gameObject.activeSelf)
                {
                    e.gameObject.SetActive(true);
                }
                e.uvRect = new Rect(e.uvRect.position + Vector2.right * scrollSpeed * Time.deltaTime, e.uvRect.size);
            }
        }
    }

    public Dictionary<int, List<RawImage>> scrollingImages;

    private void Awake()
    {
        Instace = this;
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
