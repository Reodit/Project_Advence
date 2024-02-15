using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class LevelUpPopUp : UIBase
{
    [SerializeField] private UpgradeOptionPicker upgradeOptionPicker;
 
    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void Start()
    {
        upgradeOptionPicker.PickUpgradeOptions();
        GameManager.Instance.PauseGame();
    }

    private void OnDestroy()
    {
        base.OnDisable();
        GameManager.Instance.ResumeGame();
    }
}
