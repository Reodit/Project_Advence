using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 

public class InGameUI : UIBase
{
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;

    public GameObject phaseBgPrefab;
    public GameObject phaseBgParent;

    public RectTransform playerIcon;
    public Sprite FinalBossImage;
    public Sprite BossImage;

    public GameObject phaseTargetPrefab;
    public GameObject phaseTargetParent;
    
    protected override void Update()
    {
        UpdateUIs();
    }

    public void UpdateUIs()
    {
        levelText.text = GameManager.instance.PlayerMove.currentLvl == GameManager.instance.PlayerMove.characterData.maxLv ? 
            "Max Lv" : $"Lv.{GameManager.instance.PlayerMove.currentLvl}";
        expText.text = $"{GameManager.instance.PlayerMove.currentExp} / " +
                       $"{Datas.GameData.DTCharacterLevelData[GameManager.instance.PlayerMove.currentLvl].reqExp}";
        // 진척도 = 진행 초 / 360
        var progress= 1 - StageManager.instance.currentPhase.remainTime / StageManager.instance.currentPhase.phaseTime;
        // playerIcon
        // playerIcon.anchoredPosition 
        // float xValue = (GameManager.Instance.phaseCountInCurrentStage - GameManager.Instance.currentPhaseNumber) * 300 + (progress * 300);
        float xValue = progress * 900;
        playerIcon.anchoredPosition = new Vector2(xValue, playerIcon.anchoredPosition.y);
        
        // 페이즈 진척도 개선
        phaseBgList[StageManager.instance.phaseCountInCurrentStage - StageManager.instance.currentPhase.phaseData.phaseNumber].fillAmount =
            progress;
    }

    private List<Image> phaseBgList = new List<Image>();

    protected override void Initialize()
    {
        base.Initialize();
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        uiType = UIType.HUD;
        phaseBgList.Clear();
        int phaseCountInCurrentStage = StageManager.instance.phaseCountInCurrentStage;
        for (int i = 1; i <= phaseCountInCurrentStage; i++)
        {
            var phaseBg = Instantiate(phaseBgPrefab, phaseBgParent.transform);
            phaseBgList.Add(phaseBg.GetComponent<Image>());    
            float newPositionX = phaseBgParent.GetComponent<RectTransform>().rect.width / 
                phaseCountInCurrentStage * i;
            phaseBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPositionX, phaseBg.GetComponent<RectTransform>().anchoredPosition.y);
            RectTransform rectTransform = phaseBg.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width * (4 - phaseCountInCurrentStage), rectTransform.sizeDelta.y);

            var phaseTarget = Instantiate(phaseTargetPrefab, phaseTargetParent.transform);
            phaseTarget.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(900 / phaseCountInCurrentStage * i, phaseTarget.GetComponent<RectTransform>().anchoredPosition.y);
            phaseTarget.GetComponent<Image>().sprite = (i == phaseCountInCurrentStage) ? FinalBossImage : BossImage;
            // phaseTarget의 RectTransform 컴포넌트를 가져옴
        }
    }
}
