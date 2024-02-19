using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;

public class FamiliarController : MonoBehaviour
{
    private List<(Familiar, GameObject)> _familiarAndFamiliarPrefabs;
    
    private void Start()
    {
        SkillManager.instance.OnAddSkillFamiliar += AddSkillCallback;
        SkillManager.instance.OnAddEnchantFamiliar += AddEnchantCallback;
        _familiarAndFamiliarPrefabs = new List<(Familiar, GameObject)>();
        //    StartCoroutine(FireContinuously());
    }

    private void OnDestroy()
    {
        SkillManager.instance.OnAddSkillFamiliar -= AddSkillCallback;
        SkillManager.instance.OnAddEnchantFamiliar -= AddEnchantCallback;
    }

    private void Update()
    {
        for (int i = 0; i < _familiarAndFamiliarPrefabs.Count; i++)
        {
            if(TimeManager.Instance.IsCoolTimeFinished
                   (_familiarAndFamiliarPrefabs[i].Item1.familiarData.index.ToString()))
            {
                Instantiate(_familiarAndFamiliarPrefabs[i].Item1, this.transform.position, _familiarAndFamiliarPrefabs[i].Item1.transform.rotation);
                TimeManager.Instance.Use(_familiarAndFamiliarPrefabs[i].Item1.familiarData.index.ToString());
            }
        }
    }

    public void AddSkillCallback(SkillTable skill)
    {
        var familiarData = Datas.GameData.DTFamiliarData
            .Select(pair => pair.Value)
            .FirstOrDefault(familiarData => familiarData.skillId == skill.index);

        var familiarObject = Resources.Load<GameObject>(skill.prefabPath);
        var familiar = familiarObject.GetComponent<Familiar>();

        if (familiarObject == null) 
        { 
            return; 
        }

        _familiarAndFamiliarPrefabs.Add((familiar, familiarObject));
                
        familiar.familiarData = familiarData;
        familiar.familiarSkillData = skill;

        TimeManager.Instance.RegisterCoolTime(familiar.familiarData.index.ToString(), 
            1 / SkillManager.instance.PlayerAttackSpeed(skill.index));
    }

    public void AddEnchantCallback(int skillIndex, SkillEnchantTable enchant)
    {
        var skill = Datas.PlayerData.GetCharacterSkills()[skillIndex];
        
        if (skill == null)
        {
            return;
        }
        
        var familiar = _familiarAndFamiliarPrefabs.Find(x => 
            x.Item1.familiarSkillData.index == skillIndex).Item1;
        
        switch (enchant.enchantEffect1)
        {
            case Status.AttackDamage:
                switch (skill.skillTable.type)
                {
                    case SkillType.MeleeFamiliar:
                        familiar.familiarData.maxHp =+ (familiar.familiarData.maxHp * 
                                                         enchant.enchantEffectValue1 * 
                                                         enchant.currentCount);
                        break;

                    case SkillType.RangeFamiliar:
                        familiar.bulletController.AddEnchantCallback(
                            Datas.GameData.DTSkillData[familiar.familiarData.familiarSkillId].index, enchant);
                        break;
                }
                
                break;
            
            case Status.AttackSpeed:
                TimeManager.Instance.RegisterCoolTime(familiar.familiarData.index.ToString(),
                    1 / SkillManager.instance.PlayerAttackSpeed(skillIndex));
                break;
            
            default:
                break;
        }
    }

}
