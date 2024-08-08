using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    
    private void Awake()
    {
        Initialize();   
    }

    private void Initialize()
    {
        backButton.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
    }
}
