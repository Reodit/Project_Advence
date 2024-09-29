using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]

public class ImageScrolling : MonoBehaviour
{
    public static ImageScrolling Instance;
    public Dictionary<(int, int), RawImage[]> ScrollingImages;
    
    private void Awake()
    {
        Instance = this;
        ScrollingImages = new Dictionary<(int, int), RawImage[]>();
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
    }

    // TODO 업데이트가 아니라 스테이지를 파라미터로 던지는 함수 필요
    private void Update()
    {
        if (!GameManager.instance.IsGamePaused)
        {
            UpdateBackgroundUV(StageManager.instance.currentPhase.phaseData.stage, StageManager.instance.currentPhase.phaseData.phaseNumber);
        }
    }

    private void Start()
    {
        if (StageManager.instance.stageDictionary != null)
        {
            foreach (var stage in StageManager.instance.stageDictionary)
            {
                foreach (var phase in stage.Value)
                {
                    var background = Resources.LoadAll<Texture2D>(phase.phaseData.backgroundLocal);
                    RawImage[] rawImages = new RawImage[background.Length]; 
                    for (int i = background.Length - 1; i >= 0; i--)
                    {
                        var backgroundPrefab = Resources.Load<GameObject>("UIPrefabs/ScrollingBackground");
                        var rawImageInstance = Instantiate(backgroundPrefab, this.gameObject.transform).GetComponent<RawImage>();
                        rawImageInstance.texture = background[i];
                        rawImageInstance.color = Color.white;
                        rawImageInstance.gameObject.SetActive(false);
                        rawImages[i] = rawImageInstance;
                    }

                    ScrollingImages[(stage.Key, phase.phaseData.phaseNumber)] = rawImages;
                }
            }
        }
    }

    private void UpdateBackgroundUV(int stageNumber, int phaseNumber)
    {
        foreach (var e in ScrollingImages[(stageNumber, phaseNumber)])
        {
            if (!e.gameObject.activeSelf)
            {
                e.gameObject.SetActive(true);
            }
            
            e.uvRect = new Rect(
                e.uvRect.position + 
                Vector2.right * (Time.deltaTime), e.uvRect.size);
        }
    }
}
