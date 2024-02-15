using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeOptionPicker : MonoBehaviour
{
    public RectTransform upgradeContentsParent;
    public GameObject upgradeContentsPrefab;
    
    // TODO 이미지 넣기
    public List<Image> equipSkillImages;

    private static readonly System.Random Rng = new ();
    private List<KeyValuePair<int, SkillTable>> _skillTableClone;
    private List<KeyValuePair<int, SkillEnchantTable>> _skillEnchantClone;
    private List<KeyValuePair<int, SelectStatTable>> _statUpgradeListClone;
    [SerializeField] private int maxPickCount = 3;

    private void Awake()
    {
        UpgradeDataSetUp();
    }

    private void UpgradeDataSetUp()
    {
        _skillTableClone =
            new Dictionary<int, SkillTable>(Datas.GameData.DTSkillData).ToList();
        _skillEnchantClone =
            new Dictionary<int, SkillEnchantTable>(Datas.GameData.DTSkillEnchantData).ToList();
        _statUpgradeListClone =
            new Dictionary<int, SelectStatTable>(Datas.GameData.DTSelectStatData).ToList();

        for (int i = _skillTableClone.Count - 1; i >= 0; i--)
        {
            var skillDataDict = _skillTableClone[i];

            foreach (var characterSkill in Datas.PlayerData.GetCharacterSkills())
            {
                if (skillDataDict.Value.index == characterSkill.Value.skillTable.index)
                {
                    _skillTableClone.RemoveAt(i);
                    break;
                }
            }
        }

        _skillEnchantClone = _skillEnchantClone.Where(x =>
            x.Value.currentCount < x.Value.maxCnt).ToList();

        _statUpgradeListClone = _statUpgradeListClone.Where(x =>
            x.Value.currentUpgradeCount < x.Value.maxCount).ToList();
    }

    public void PickUpgradeOptions()
    {
        if (Datas.PlayerData.GetCharacterSkills().Count == 0)
        {
            OfferNewSkills();
        }
        
        else
        {
            OfferUpgrades();
        }
    }

    private void OfferNewSkills()
    {
        var selectedSkills = Datas.GameData.DTSkillData.
            OrderBy(x => Rng.Next()).Take(maxPickCount).ToList();
        foreach (var skill in selectedSkills)
        {
            SetupUpgradeUI(skill.Value, () =>
            {
                SkillManager.instance.AddPlayerSkill(skill.Value);
                _skillTableClone.Remove(skill);
            });
        }
    }

    private void OfferUpgrades()
    {
        int pickCount = 0;

        while (pickCount < maxPickCount)
        {
            if (Datas.PlayerData.GetCharacterSkills().Count < maxPickCount)
            {
                OfferSkillOrStatOrSkillEnchantUpgrade();
                pickCount++;
            }

            else
            {
                OfferSkillEnchantOrStatUpgrade();
                pickCount++;
            }
        }
    }

    private void OfferSkillOrStatOrSkillEnchantUpgrade()
    {    
        int choice = Rng.Next(Datas.GameData.DTSkillData.Count + 
            Datas.GameData.DTSkillEnchantData.Count + 
            Datas.GameData.DTSelectStatData.Count);

        if (choice < Datas.GameData.DTSkillData.Count)
        {
            OfferNewSkillOption();
        }
        
        else if (choice < (Datas.GameData.DTSkillData.Count + Datas.GameData.DTSkillEnchantData.Count))
        {
            OfferSkillEnchantOption();
        }
        
        else
        {
            OfferStatUpgradeOption();
        }
    }

    private void OfferSkillEnchantOrStatUpgrade()
    {
        int choice = Rng.Next(Datas.GameData.DTSkillEnchantData.Count + 
            Datas.GameData.DTSelectStatData.Count);
        
        if (choice < Datas.GameData.DTSkillEnchantData.Count)
        {
            OfferSkillEnchantOption();
        }
        
        else
        {
            OfferStatUpgradeOption();
        }
    }

    private void SetupUpgradeUI(SkillTable skillData, Action onClickAction)
    {
        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(skillData.icon);
        upgradePrefab.upgradeTitleText.text = skillData.name;

        upgradePrefab.upgradeDescriptionText.text = ExcelUtility.FormatStringWithVariables(skillData.description);
        upgradePrefab.upgradeButton.onClick.AddListener(() =>
        {
            onClickAction.Invoke();
            Destroy(this.gameObject);
        });
    }

    private void SetupUpgradeUI(SkillTable skillData, SkillEnchantTable skillEnchantData, Action onClickAction)
    {
        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(skillEnchantData.icon);
        upgradePrefab.upgradeTitleText.text = skillEnchantData.name;

        upgradePrefab.upgradeDescriptionText.text = ExcelUtility.FormatStringWithVariables(skillEnchantData.description, skillData.name, skillEnchantData.enchantEffectValue1);
        upgradePrefab.upgradeButton.onClick.AddListener(() =>
        {
            onClickAction.Invoke();
            Destroy(this.gameObject);
        });
    }

    private void SetupUpgradeUI(SelectStatTable selectStatData, Action onClickAction)
    {
        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(selectStatData.iconPath);
        upgradePrefab.upgradeTitleText.text = selectStatData.name;

        upgradePrefab.upgradeDescriptionText.text = ExcelUtility.FormatStringWithVariables(selectStatData.description);
        upgradePrefab.upgradeButton.onClick.AddListener(() =>
        {
            onClickAction.Invoke();
            Destroy(this.gameObject);
        });
    }

    private void OfferNewSkillOption()
    {
        if (_skillTableClone.Count == 0)
        {
            return;
        }
        
        var pickRandomSkill = _skillTableClone[Random.Range(0, _skillTableClone.Count - 1)];
        
        var upgradePrefab = Instantiate(upgradeContentsPrefab, upgradeContentsParent).GetComponent<UpgradeContentsUI>();
        upgradePrefab.upgradeIcon.sprite = Resources.Load<Sprite>(pickRandomSkill.Value.icon);
                        
        upgradePrefab.upgradeTitleText.text = pickRandomSkill.Value.name;
        upgradePrefab.upgradeDescriptionText.text = pickRandomSkill.Value.description;
        upgradePrefab.upgradeButton.onClick.AddListener(() =>
        {
            SkillManager.instance.AddPlayerSkill(pickRandomSkill.Value);
            Destroy(this.gameObject);
        }); 
        
        _skillTableClone.Remove(pickRandomSkill);
    }

    private void OfferSkillEnchantOption()
    {
        var pickRandomSkillInPlayer = Datas.PlayerData.GetCharacterSkills().ToList()
            [Random.Range(0, Datas.PlayerData.GetCharacterSkills().Count)].Value;
        
        var pickRandomSkillEnchant = 
            _skillEnchantClone[Random.Range(0, _skillEnchantClone.Count - 1)];

        SetupUpgradeUI(pickRandomSkillInPlayer.skillTable, pickRandomSkillEnchant.Value,
            () =>
            {
                SkillManager.instance.AddSkillEnchant(pickRandomSkillInPlayer.skillTable.index, 
                    pickRandomSkillEnchant.Value);
                Destroy(this.gameObject);
            });

        _skillEnchantClone.Remove(pickRandomSkillEnchant);
        // 아이콘 배치 필요
    }

    private void OfferStatUpgradeOption()
    {
        var pickRandomStatUpgrade = 
            _statUpgradeListClone[Random.Range(0, _statUpgradeListClone.Count - 1)];

        SetupUpgradeUI(pickRandomStatUpgrade.Value, () =>
        {
            Datas.PlayerData.GetUpgradeHistory().selectStatTable.Add(
                pickRandomStatUpgrade.Value);
            Destroy(this.gameObject);
        });
        
        _statUpgradeListClone.Remove(pickRandomStatUpgrade);
    }
}