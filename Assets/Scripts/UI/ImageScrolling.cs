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

        if (StageManager.instance.StageDictionary != null)
        {
            foreach (var stage in StageManager.instance.StageDictionary)
            {
                foreach (var phase in stage.Value)
                {
                    var background = Resources.LoadAll<RawImage>(phase.phaseData.backgroundLocal);
                    ScrollingImages[(stage.Key, phase.phaseData.phaseNumber)] = background;
                }
            }
        }
    }
    
    // TODO 업데이트가 아니라 스테이지를 파라미터로 던지는 함수 필요
    private void Update()
    {
        if (!GameManager.instance.IsGamePaused)
        {
            UpdateBackgroundUV(StageManager.instance.currentPhase.phaseData.stage, StageManager.instance.currentPhase.phaseData.phaseNumber);
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
                Vector2.right * (StageManager.instance.StageDictionary[stageNumber][phaseNumber].phaseData.scrollSpeed * Time.deltaTime), e.uvRect.size);
        }
    }
}
