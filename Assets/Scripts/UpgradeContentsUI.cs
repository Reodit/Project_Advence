using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContentsUI : UIBase
{
    public TextMeshProUGUI upgradeTitleText;
    public TextMeshProUGUI upgradeDescriptionText;
    public Image upgradeIcon;
    public Button upgradeButton;
    protected override void Start()
    {
        base.Start();
    }

    // public void UpdateContents(string title, string desc, Sprite icon, )
    // {
    //     upgradeTitleText.text = title;
    //     upgradeDescriptionText.text = desc;
    //     upgradeIcon.sprite = icon;
    //     upgradeButton.onClick.AddListener();
    // }
}
