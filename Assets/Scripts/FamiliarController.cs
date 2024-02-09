using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FamiliarController : MonoBehaviour
{
    private List<Familiar> _familiars;
    private List<GameObject> _familiarPrefabs; 
    private void Start()
    {
        SkillManager.instance.OnAddSkillFamiliar += AddSkillCallback;
        SkillManager.instance.OnAddEnchantFamiliar += AddEnchantCallback;


    //    StartCoroutine(FireContinuously());
    }

    private void OnDestroy()
    {
        SkillManager.instance.OnAddSkillFamiliar -= AddSkillCallback;
        SkillManager.instance.OnAddEnchantFamiliar -= AddEnchantCallback;
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            foreach (var e in _familiarPrefabs) 
            {
                if (e)
                {
                    
                }
            }

            yield return null;
        }
    }

    public void AddSkillCallback(SkillTable skill)
    {
        var familiarData = Datas.GameData.DTFamiliarData
            .Select(pair => pair.Value)
            .FirstOrDefault(familiarData => familiarData.SkillId == skill.index);

        var familiarObject = Resources.Load<GameObject>(skill.prefabPath);
        var familiar = familiarObject.GetComponent<Familiar>();

        if (familiarObject == null) 
        { 
            return; 
        }

        _familiarPrefabs.Add(familiarObject);
        _familiars.Add(familiar);
                
        familiar.familiarData = familiarData;
        familiar.familiarSkillData = skill;

        if (familiarData == null)
        {
            return;
        }

        bool isMeleeFamiliar = false;

        // TODO Ÿ���� ���� �÷����� ����
        if (familiarData.MaxHp == 0)
        {
            isMeleeFamiliar = false;
        }

        else
        {
            isMeleeFamiliar = true;
        }

        if (!isMeleeFamiliar)
        {
            familiar.bulletController.AddSkillCallback(skill);
        }
    }

    public void AddEnchantCallback(string skillName, SkillEnchantTable enchant)
    {
        // 1. skillName�� �ش��ϴ� ��ų�� ã��, �ش� ��ų�� � Ÿ������ Ȯ�� 
        // 2. Ÿ�Կ� �ش��ϴ� ��æƮ�� �ο�

        var skill = Datas.GameData.DTSkillData
            .Select(pair => pair.Value)
            .FirstOrDefault(skill => skill.name == skillName);
        
        if (skill == null)
        {
            return;
        }

        var familiarData = Datas.GameData.DTFamiliarData
            .Select(pair => pair.Value)
            .FirstOrDefault(familiarData => familiarData.SkillId == skill.index);

        if (familiarData == null)
        {
            return;
        }

        bool isMeleeFamiliar = false;

        // TODO Ÿ���� ���� �÷����� ����
        if (familiarData.MaxHp == 0)
        {
            isMeleeFamiliar = false;
        }

        else
        {
            isMeleeFamiliar = true;
        }


        if (isMeleeFamiliar)
        {
            // async status;
            // damage, attackspeed ==> spawntime
            foreach(var e in SkillManager.instance.playerSkills[skillName].SkillEnchantTables)
            {
                if (e != null && e.currentCount < e.maxCnt)
                {
                    switch(e.enchantEffect1)
                    {
                        case EnchantEffect1.SkillDamageControl:
                            foreach (var familiar in _familiars)
                            {
                                if (familiar.FamiliarType == FamiliarType.melee)
                                {
                                    familiar.familiarData.MaxHp = familiarData.MaxHp + e.currentCount * (e.enchantEffectValue1 * familiarData.MaxHp);
                                    familiar.currentHp = e.currentCount * (e.enchantEffectValue1 * familiarData.MaxHp);
                                }
                            }
                            break;

                        case EnchantEffect1.AttackSpeedControl:
                            foreach (var familiar in _familiars)
                            {
                                if (familiar.FamiliarType == FamiliarType.melee)
                                {
                                    familiar.spawnCoolTime = (1 / (Datas.GameData.DTCharacterData[0].attackSpeed + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[0].attackSpeed)));
                                }
                            }
                            break;
                        default: 
                            break;
                    }
                }
            }
        }

        else
        {
            foreach (var e in SkillManager.instance.playerSkills[skillName].SkillEnchantTables)
            {
                if (e != null && e.currentCount < e.maxCnt)
                {
                    switch (e.enchantEffect1)
                    {
                        case EnchantEffect1.AttackSpeedControl:
                            foreach (var familiar in _familiars)
                            {
                                if (familiar.FamiliarType == FamiliarType.range)
                                {
                                    familiar.spawnCoolTime = (1 / (Datas.GameData.DTCharacterData[0].attackSpeed + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[0].attackSpeed)));
                                }
                            }

                            break;

                        default:
                            foreach (var familiar in _familiars)
                            {
                                if (familiar.FamiliarType == FamiliarType.range)
                                {
                                    familiar.bulletController.AddEnchantCallback(Datas.GameData.DTSkillData[familiarData.SkillId].name, enchant);
                                }
                            }

                            break;
                    }
                }
            }
        }
    }

}
