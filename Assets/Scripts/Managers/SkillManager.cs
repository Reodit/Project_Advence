using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
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
        var playerSkills = Datas.PlayerData.GetCharacterSkills();
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
        var playerSkills = Datas.PlayerData.GetCharacterSkills();
        
        CharacterSkill characterSkill = playerSkills[skillIndex];
        int enchantIndex = playerSkills[skillIndex].skillEnchantTables.IndexOf(skillEnchant);
        if (enchantIndex == -1)
        {
            characterSkill.skillEnchantTables.Add(skillEnchant);
            enchantIndex = characterSkill.skillEnchantTables.Count - 1;
        }
        else if (skillEnchant.maxCnt <= characterSkill.skillEnchantTables[enchantIndex].currentCount)
        {
            return;
        }

        characterSkill.skillEnchantTables[enchantIndex].currentCount++;
        
        if (playerSkills[skillIndex].skillTable.type == Enums.SkillType.Creature)
        {
            OnAddEnchantFamiliar.Invoke(skillIndex, skillEnchant);
        }
        else
        {
            OnAddEnchant.Invoke(skillIndex, skillEnchant);
        }
    }
}
