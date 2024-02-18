using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        if (familiarData == null)
        {
            return;
        }

        bool isMeleeFamiliar = false;

        // TODO Ÿ���� ���� �÷����� ����
        if (familiarData.maxHp == 0)
        {
            isMeleeFamiliar = false;
        }

        else
        {
            isMeleeFamiliar = true;
        }

        familiar.spawnCoolTime = Datas.GameData.DTCharacterData[1].attackSpeed;
        TimeManager.Instance.RegisterCoolTime(familiar.familiarData.index.ToString(), familiar.spawnCoolTime);
        
        if (isMeleeFamiliar == false)
        {
            familiar.bulletController.AddSkillCallback(skill);
        }
    }

    public void AddEnchantCallback(int skillIdnex, SkillEnchantTable enchant)
    {
        // 1. skillName�� �ش��ϴ� ��ų�� ã��, �ش� ��ų�� � Ÿ������ Ȯ�� 
        // 2. Ÿ�Կ� �ش��ϴ� ��æƮ�� �ο�

        var skill = Datas.GameData.DTSkillData
            .Select(pair => pair.Value)
            .FirstOrDefault(skill => skill.index == skillIdnex);
        
        if (skill == null)
        {
            return;
        }

        var familiarData = Datas.GameData.DTFamiliarData
            .Select(pair => pair.Value)
            .FirstOrDefault(familiarData => familiarData.skillId == skill.index);

        if (familiarData == null)
        {
            return;
        }

        bool isMeleeFamiliar = false;

        // TODO Ÿ���� ���� �÷����� ����
        if (familiarData.maxHp == 0)
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
            foreach(var e in Datas.PlayerData.GetCharacterSkills()[skillIdnex].skillEnchantTables)
            {
                if (e != null && e.currentCount < e.maxCnt)
                {
                    switch(e.enchantEffect1)
                    {
                        case EnchantEffect1.SkillDamageControl:
                            foreach (var familiar in _familiarAndFamiliarPrefabs)
                            {
                                if (familiar.Item1.familiarType == FamiliarType.melee)
                                {
                                    familiar.Item1.familiarData.maxHp = (int)(familiarData.maxHp + e.currentCount * (e.enchantEffectValue1 * familiarData.maxHp));
                                    familiar.Item1.currentHp = (int)(e.currentCount * (e.enchantEffectValue1 * familiarData.maxHp));
                                }
                            }
                            break;

                        case EnchantEffect1.AttackSpeedControl:
                            foreach (var familiar in _familiarAndFamiliarPrefabs)
                            {
                                if (familiar.Item1.familiarType == FamiliarType.melee)
                                {
                                    familiar.Item1.spawnCoolTime = Datas.GameData.DTCharacterData[1].attackSpeed + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[1].attackSpeed);
                                    TimeManager.Instance.RegisterCoolTime(familiar.Item1.familiarData.index.ToString(), familiar.Item1.spawnCoolTime);
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
            foreach (var e in Datas.PlayerData.GetCharacterSkills()[skillIdnex].skillEnchantTables)
            {
                if (e != null && e.currentCount < e.maxCnt)
                {
                    switch (e.enchantEffect1)
                    {
                        case EnchantEffect1.AttackSpeedControl:
                            foreach (var familiar in _familiarAndFamiliarPrefabs)
                            {
                                if (familiar.Item1.familiarType == FamiliarType.range)
                                {
                                    familiar.Item1.spawnCoolTime = Datas.GameData.DTCharacterData[1].attackSpeed + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[1].attackSpeed);
                                    TimeManager.Instance.RegisterCoolTime(familiar.Item1.familiarData.index.ToString(), familiar.Item1.spawnCoolTime);
                                }
                            }

                            break;

                        default:
                            foreach (var familiar in _familiarAndFamiliarPrefabs)
                            {
                                if (familiar.Item1.familiarType == FamiliarType.range)
                                {
                                    familiar.Item1.bulletController.AddEnchantCallback(Datas.GameData.DTSkillData[familiarData.skillId].index, enchant);
                                }
                            }

                            break;
                    }
                }
            }
        }
    }

}
