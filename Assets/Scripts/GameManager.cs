using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public IngameUI IngameUI;
    public PlayerMove PlayerMove;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
