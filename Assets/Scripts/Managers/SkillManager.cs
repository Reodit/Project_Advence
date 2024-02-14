using System;
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
    
    // TODO 소환수 관련 리펙토링
    public event Action<SkillTable> OnAddSkillFamiliar;
    public event Action<string, SkillEnchantTable> OnAddEnchantFamiliar;


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
        
        if (!(skill.name == "고스트나이트" || skill.name == "미니페어리"))
        {
            _debugger.Spawn(skill.index);
        }
#endif

        if (playerSkills.ContainsKey(skill.name))
            return;

        playerSkills.Add(skill.name, new CharacterSkill(skill, new List<SkillEnchantTable>()));

        if (skill.name == "고스트나이트" || skill.name == "미니페어리")
        {
            OnAddSkillFamiliar.Invoke(skill);
        }

        else
        {
            OnAddSkill.Invoke(skill);   
        } 
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
        
        if (skillName == "고스트나이트" || skillName == "미니페어리")
        {
            OnAddEnchantFamiliar.Invoke(skillName, skillEnchant);
        }

        else
        {
            OnAddEnchant.Invoke(skillName, skillEnchant);
        }
    }
}
