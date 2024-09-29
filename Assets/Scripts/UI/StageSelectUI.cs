using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button gameStartButton;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (var stage in StageManager.instance.stageDictionary)
        {
            // TODO 유저 언락 데이터에 stage 클리어 여부 (int) / (bool) 남기고 unlock 해주기
            
        }   
        
        backButton.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });
        
        gameStartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("IngameScene");
        });
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.RemoveAllListeners();
    }
}
