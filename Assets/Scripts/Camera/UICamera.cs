using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    public static UICamera Instance;

    UICamera() { }

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Camera uiCamera = GetComponent<Camera>();
        //uiCamera.cullingMask = (1 << 5) | (1 << 6);
    }
    
    
}
