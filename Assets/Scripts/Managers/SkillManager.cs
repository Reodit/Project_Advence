using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dictionary<int, CharacterSkill> playerSkills = new Dictionary<int, CharacterSkill>();
    public UpgradeHistory playerUpgradeHistory = new UpgradeHistory(new List<SelectStatTable>());

    public event Action<SkillTable> OnAddSkill;
    public event Action<int, SkillEnchantTable> OnAddEnchant;
    
    // TODO 소환수 관련 리펙토링
    public event Action<SkillTable> OnAddSkillFamiliar;
    public event Action<int, SkillEnchantTable> OnAddEnchantFamiliar;

    private void Awake()
    {
        instance = this;
    }

    public void AddPlayerSkill(SkillTable skill)
    {
        if (playerSkills.ContainsKey(skill.index))
            return;

        bool isCreature = skill.type == Enums.SkillType.Creature;

        playerSkills.Add(skill.index, new CharacterSkill(skill, new List<SkillEnchantTable>()));

        if (isCreature)
        {
            OnAddSkillFamiliar.Invoke(skill);
        }
        else
        {
            OnAddSkill.Invoke(skill);   
        } 
    }

    public void AddSkillEnchant(int skillIndex, SkillEnchantTable skillEnchant)
    {
        CharacterSkill characterSkill = playerSkills[skillIndex];
        int enchantIndex = playerSkills[skillIndex].SkillEnchantTables.IndexOf(skillEnchant);
        if (enchantIndex == -1)
        {
            characterSkill.SkillEnchantTables.Add(skillEnchant);
            enchantIndex = characterSkill.SkillEnchantTables.Count - 1;
        }
        else if (skillEnchant.maxCnt <= characterSkill.SkillEnchantTables[enchantIndex].currentCount)
        {
            return;
        }

        characterSkill.SkillEnchantTables[enchantIndex].currentCount++;
        
        if (playerSkills[skillIndex].SkillTable.type == Enums.SkillType.Creature)
        {
            OnAddEnchantFamiliar.Invoke(skillIndex, skillEnchant);
        }
        else
        {
            OnAddEnchant.Invoke(skillIndex, skillEnchant);
        }
    }
}
