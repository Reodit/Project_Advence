using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button statusButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private TextMeshProUGUI goldAmount;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        statusButton.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>("UIPrefabs/UI_Store"));
        });
        startButton.onClick.AddListener(() =>
        {
            // Go Stage Select UI
        });
        settingButton.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>("UIPrefabs/UI_Ingame_Setting_Popup"));
        });
        
    }
    
    private void OnDestroy()
    {
        statusButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();
        settingButton.onClick.RemoveAllListeners();
    }
}
