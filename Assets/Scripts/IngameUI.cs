using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
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

    public void Start()
    {
    }

    public void Update()
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
                      GameManager.Instance.MonsterSpawner.currentPhase.PhaseTime;
        // playerIcon
        //playerIcon.anchoredPosition 
        float xValue = (GameManager.Instance.phaseCount - GameManager.Instance.MonsterSpawner.Phases.Count) * 300 + (progress * 300);
        playerIcon.anchoredPosition = new Vector2(xValue, playerIcon.anchoredPosition.y);
        
        // 페이즈 진척도 개선
        phaseBgsList[GameManager.Instance.phaseCount - GameManager.Instance.MonsterSpawner.Phases.Count].fillAmount =
            progress;

    }

    private List<Image> phaseBgsList = new List<Image>();
    
    public void Init()
    {
        GameManager.Instance.IngameUI = this;
        phaseBgsList.Clear();
        for (int i = 1; i <= GameManager.Instance.phaseCount; i++)
        {
            var phaseBg = Instantiate(phaseBgPrefab, phaseBgParent.transform);
            phaseBgsList.Add(phaseBg.GetComponent<Image>());
            float newPositionX = phaseBgParent.GetComponent<RectTransform>().rect.width / GameManager.Instance.phaseCount * i;
            phaseBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPositionX, phaseBg.GetComponent<RectTransform>().anchoredPosition.y);
            var phaseTarget = Instantiate(phaseTargetPrefab, phaseTargetParent.transform);
            phaseTarget.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(300 * i, phaseTarget.GetComponent<RectTransform>().anchoredPosition.y);
            phaseTarget.GetComponent<Image>().sprite = (i == GameManager.Instance.phaseCount) ? FinalBossImage : BossImage;
            
        }
    }
}
