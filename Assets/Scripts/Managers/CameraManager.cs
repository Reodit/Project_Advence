using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera { get; private set; }
    // public Camera uiCamera { get; private set; }
    // public readonly List<Camera> stackCameras;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }
}
