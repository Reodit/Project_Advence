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

    private void Awake()
    {
        instance = this;
    }

    public void AddPlayerSkill(SkillTable skill)
    {
        if (playerSkills.ContainsKey(skill.name))
            return;

        playerSkills.Add(skill.name, new CharacterSkill(skill, new List<SkillEnchantTable>()));
    }



    public void AddSkillEnchant(string skillName, SkillEnchantTable skillEnchant)
    {
        playerSkills[skillName].SkillEnchantTables.Add(skillEnchant);
    }

    public void AddSkillEnchant(int skillIndex, SkillEnchantTable skillEnchant)
    {
        _upgradableskills = new List<CharacterSkill>(playerSkills.Values);

        _upgradableskills[skillIndex].SkillEnchantTables.Add(skillEnchant);
    }
}