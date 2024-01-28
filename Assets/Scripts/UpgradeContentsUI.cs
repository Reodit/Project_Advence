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

    protected override void Start()
    {
        base.Start();
        upgradeTitleText.text = "";
        upgradeDescriptionText.text = "";
        upgradeIcon.sprite = null;
    }
}
