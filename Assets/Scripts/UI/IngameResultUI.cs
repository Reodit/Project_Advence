using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameResultUI : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button nextStageButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        homeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("OutgameScene");
            PopupManager.instance.ClearPopup();
            StageManager.instance.Initialize();
            StageManager.instance.LoadStage(1);
        });
        
        nextStageButton.onClick.AddListener(() =>
        {
            StageManager.instance.Initialize();
            PopupManager.instance.ClearPopup();
            StageManager.instance.LoadStage(StageManager.instance.currentPhase.phaseData.stage);
            StageManager.instance.isStopSpawn = true;
        });
        
        scoreText.text = StageManager.instance.scoreCurrentStage.ToString();
        goldText.text = StageManager.instance.goldCurrentStage.ToString();
    }
}
