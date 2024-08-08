using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        closeButton.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveAllListeners();
    }
}
