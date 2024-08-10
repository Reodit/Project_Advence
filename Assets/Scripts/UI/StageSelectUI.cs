using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private List<Stage> stages = new List<Stage>();
    [SerializeField] private Button backButton;
    [SerializeField] private Button gameStartButton;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (var e in stages)
        {
            if (e.UserUnlock)
            {
                // unlock image = true
            }
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
