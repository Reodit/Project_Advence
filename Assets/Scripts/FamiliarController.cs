using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FamiliarController : MonoBehaviour
{
    private Familiar _familiar;
    private void Start()
    {
        SkillManager.instance.OnAddSkillFamiliar += AddSkillCallback;
        SkillManager.instance.OnAddEnchantFamiliar += AddEnchantCallback;
        _familiar = GetComponent<Familiar>();
    }

    private void OnDestroy()
    {
        SkillManager.instance.OnAddSkillFamiliar -= AddSkillCallback;
        SkillManager.instance.OnAddEnchantFamiliar -= AddEnchantCallback;
    }

    public void AddSkillCallback(SkillTable skill)
    {
        var familiarData = Datas.GameData.DTFamiliarData
            .Select(pair => pair.Value)
            .FirstOrDefault(familiarData => familiarData.SkillId == skill.index);

        _familiar.familiarData = familiarData;
        _familiar.familiarSkillData = skill;
    }

    public void AddEnchantCallback(string skillName, SkillEnchantTable enchant)
    {
        
    }
}
