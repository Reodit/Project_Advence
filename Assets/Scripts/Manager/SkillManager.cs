using Enums;
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

#if UNITY_EDITOR
    private SkillUpgradeDebugger _debugger;
#endif

    private void Awake()
    {
        instance = this;
#if UNITY_EDITOR
        _debugger = FindAnyObjectByType<SkillUpgradeDebugger>();
#endif
    }

    public void AddPlayerSkill(SkillTable skill)
    {
#if UNITY_EDITOR
        _debugger.Spawn(skill.index);
#endif

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
            index = playerSkills[skillName].SkillEnchantTables.Count - 1;
        }
        else if (skillEnchant.maxCnt <= playerSkills[skillName].SkillEnchantTables[index].currentCount)
        {
            return;
        }

        playerSkills[skillName].SkillEnchantTables[index].currentCount++;
        OnAddEnchant.Invoke(skillName, skillEnchant);
    }

    
}
