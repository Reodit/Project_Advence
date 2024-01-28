using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPopUp : UIBase
{
    public RectTransform upgradeContentsParent;
    public GameObject upgradeContentsPrefab;
    
    public List<Image> equipSkillImages;

    protected override void Initialize()
    {
        base.Initialize();

        //upgradeContentsPrefab.GetComponent<UpgradeContentsUI>();
    }

    protected override void Start()
    {
        GameManager.Instance.PauseGame();
    }

    private void OnDestroy()
    {
        base.OnDisable();
        GameManager.Instance.ResumeGame();
    }
}
