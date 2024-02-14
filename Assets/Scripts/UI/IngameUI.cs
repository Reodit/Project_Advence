using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 
public class IngameUI : UIBase
{
    public static IngameUI Instance;

    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;

    public GameObject phaseBgPrefab;
    public GameObject phaseBgParent;

    public RectTransform playerIcon;
    public Sprite FinalBossImage;
    public Sprite BossImage;

    public GameObject phaseTargetPrefab;
    public GameObject phaseTargetParent;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        UIManager.Instance.RegisterUIElement("IngameUI", this);
    }

    protected override void Update()
    {
        UpdateUIs();
    }

    public void UpdateUIs()
    {
        levelText.text = GameManager.Instance.PlayerMove.currentLvl == GameManager.Instance.PlayerMove.characterData.maxLv ? 
            "Max Lv" : $"Lv.{GameManager.Instance.PlayerMove.currentLvl}";
        expText.text = $"{GameManager.Instance.PlayerMove.currentExp} / " +
                       $"{Datas.GameData.DTCharacterLevelData[GameManager.Instance.PlayerMove.currentLvl].reqExp}";
        // 진척도 = 진행 초 / 360
        var progress= 1 - GameManager.Instance.MonsterSpawner.currentPhase.remainTime /
                      GameManager.Instance.MonsterSpawner.currentPhase.phaseTime;
        // playerIcon
        //playerIcon.anchoredPosition 
        //float xValue = (GameManager.Instance.phaseCountInCurrentStage - GameManager.Instance.currentPhaseNumber) * 300 + (progress * 300);
        float xValue = progress * 900;
        playerIcon.anchoredPosition = new Vector2(xValue, playerIcon.anchoredPosition.y);
        
        // 페이즈 진척도 개선
        phaseBgsList[GameManager.Instance.phaseCountInCurrentStage - GameManager.Instance.currentPhaseNumber].fillAmount =
            progress;
    }

    private List<Image> phaseBgsList = new List<Image>();
    
    public void Init()
    {
        GameManager.Instance.IngameUI = this;
        phaseBgsList.Clear();
        int phaseCountInCurrentStage = GameManager.Instance.phaseCountInCurrentStage;
        for (int i = 1; i <= phaseCountInCurrentStage; i++)
        {
            var phaseBg = Instantiate(phaseBgPrefab, phaseBgParent.transform);
            phaseBgsList.Add(phaseBg.GetComponent<Image>());    
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
