using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    public static IngameUI Instance;

    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        Instance = this;
        GameManager.Instance.IngameUI = this;
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
        levelText.text = GameManager.Instance.PlayerMove.currentLvl == GameManager.Instance.PlayerMove.maxLvl ? 
            "Max Lv" : $"Lv.{GameManager.Instance.PlayerMove.currentLvl}";
        expText.text = $"{GameManager.Instance.PlayerMove.currentExp} / " +
                       $"{GameManager.Instance.PlayerMove.needExpNextLvl[GameManager.Instance.PlayerMove.currentLvl]}";
    }
}
