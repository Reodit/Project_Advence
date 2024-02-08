using Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FamiliarController : MonoBehaviour
{
    private List<Familiar> _familiars;
    private List<GameObject> _familiarPrefabss; 
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

    /*IEnumerator FireContinuously()
    {
        while (true)
        {
            // familiar prefab 
            if (_bulletPrefabDict.Count > 0)
            {
                _spawner.SpawnFrontBullets(_frontBullets, _bulletInfoDict);
                _spawner.SpawnSlashBullets(_slashBullets, _bulletInfoDict, Angle);
            }
            
            yield return new WaitForSeconds(1f / fireRate);
        }
    }*/

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

        _familiarPrefabss.Add(familiarObject);
        _familiars.Add(familiar);
                
        familiar.familiarData = familiarData;
        familiar.familiarSkillData = skill;

        if (familiarData == null)
        {
            return;
        }

        bool isMeleeFamiliar = false;

        // TODO 타입은 따로 컬럼으로 빼기
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
        // 1. skillName에 해당하는 스킬을 찾고, 해당 스킬이 어떤 타입인지 확인 
        // 2. 타입에 해당하는 인챈트를 부여

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

        // TODO 타입은 따로 컬럼으로 빼기
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
                                    familiar.familiarData.MaxHp = Datas.GameData.DTCharacterData[0].attackDamage + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[0].attackDamage);
                                }
                            }
                            break;

                        case EnchantEffect1.AttackSpeedControl:
                            foreach (var familiar in _familiars)
                            {
                                if (familiar.FamiliarType == FamiliarType.melee)
                                {
                                    familiar.spawnCoolTime = (1 / Datas.GameData.DTCharacterData[0].attackSpeed + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[0].attackSpeed));
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
                                    familiar.spawnCoolTime = (1 / Datas.GameData.DTCharacterData[0].attackSpeed + e.currentCount * (e.enchantEffectValue1 * Datas.GameData.DTCharacterData[0].attackSpeed));
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
