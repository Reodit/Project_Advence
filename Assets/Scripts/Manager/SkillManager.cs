using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dictionary<string, CharacterSkill> playerSkills = new Dictionary<string, CharacterSkill>();
    public UpgradeHistory playerUpgradeHistory = new UpgradeHistory(new List<SelectStatTable>());

    private List<CharacterSkill> _upgradableskills = new List<CharacterSkill>();

    public event Action<SkillTable> OnAddSkill;
    public event Action<string, SkillEnchantTable> OnAddEnchant;

    private void Awake()
    {
        instance = this;
    }

    public void AddPlayerSkill(SkillTable skill)
    {
        if (playerSkills.ContainsKey(skill.name))
            return;

        playerSkills.Add(skill.name, new CharacterSkill(skill, new List<SkillEnchantTable>()));

        OnAddSkill.Invoke(skill);
    }

    public void AddSkillEnchant(string skillName, SkillEnchantTable skillEnchant)
    {
        int index = playerSkills[skillName].SkillEnchantTables.IndexOf(skillEnchant);
        if (index == -1)
        {
            playerSkills[skillName].SkillEnchantTables.Add(skillEnchant);
        }
        else if (skillEnchant.maxCnt <= playerSkills[skillName].SkillEnchantTables[index].maxCnt)
        {
            return;
        }

        int enchantCount = playerSkills[skillName].SkillEnchantTables.Count;
        playerSkills[skillName].SkillEnchantTables[enchantCount - 1].maxCnt++;
        OnAddEnchant.Invoke(skillName, skillEnchant);
    }
}
