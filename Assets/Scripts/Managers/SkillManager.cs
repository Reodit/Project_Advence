using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Datas;
using Enums;

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

        bool isFamiliar = skill.type is Enums.SkillType.MeleeFamiliar or Enums.SkillType.RangeFamiliar;

        playerSkills.Add(skill.index, new CharacterSkill(skill, new List<SkillEnchantTable>()));

        if (isFamiliar)
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
        
        bool isFamiliar = 
            playerSkills[skillIndex].skillTable.type is 
                Enums.SkillType.MeleeFamiliar or Enums.SkillType.RangeFamiliar;

        
        if (isFamiliar)
        {
            OnAddEnchantFamiliar.Invoke(skillIndex, skillEnchant);
        }
        else
        {
            OnAddEnchant.Invoke(skillIndex, skillEnchant);
        }
    }
    
    public float PlayerResultSkillDamage(int skillID, bool isCriticalApply = true)
    {
        var characterSkill = PlayerData.GetCharacterSkills()[skillID];
        var characterBaseData = GameManager.Instance.PlayerMove.characterData;
        float baseDamage = 0f;
        
        switch (characterSkill.skillTable.type)
        {
            case SkillType.MeleeFamiliar:
                baseDamage = Datas.GameData.DTFamiliarData.FirstOrDefault(familiar =>
                    familiar.Value.skillId == skillID).Value.maxHp;
                
                var skillFamiliarMaxHpEnchant = characterSkill.skillEnchantTables
                    .FirstOrDefault(e => e.enchantEffect1 == Status.SkillDamageRate);

                var skillFamiliarMaxHp = (skillFamiliarMaxHpEnchant?.enchantEffectValue1 ?? 0) * 
                                      (skillFamiliarMaxHpEnchant?.currentCount ?? 0);
                
                baseDamage += (baseDamage * skillFamiliarMaxHp);
                break;
            
            default:
                baseDamage = characterBaseData.attackDamage;
                
                var upgradeDamageRateStat = PlayerData.GetUpgradeHistory().selectStatTable.FirstOrDefault(status =>
                    status.selectStatName == Status.AttackDamage);
                
                var skillDamageRateEnchant = characterSkill.skillEnchantTables
                    .FirstOrDefault(e => e.enchantEffect1 == Status.SkillDamageRate);

                var shopAttackDamage = PlayerData.GetCharacterStats().Where(status =>
                    status.upgradeName == Status.AttackDamage); 
                
                var upgradeSkillDamageRate = (upgradeDamageRateStat?.currentUpgradeCount ?? 0) *
                                             (upgradeDamageRateStat?.selectStatRateValue ?? 0);
                
                var skillDamageRate = (skillDamageRateEnchant?.enchantEffectValue1 ?? 0) * 
                                          (skillDamageRateEnchant?.currentCount ?? 0);

                var shopAttackDamageSum = shopAttackDamage.Sum(stats => stats.addStatValue);
                baseDamage += shopAttackDamageSum;
                
                float resultDamageRate =
                    upgradeSkillDamageRate + skillDamageRate + characterSkill.skillTable.skillDamageRate;
                
                baseDamage += baseDamage * resultDamageRate;
                break;
        }
        
        if (isCriticalApply)
        {
            // 알아보기가 어려울 수 있어서 일단 다 펼쳐서 작성함.
            
            var upgradeCriticalChanceStat = PlayerData.GetUpgradeHistory().selectStatTable.FirstOrDefault(status =>
                status.selectStatName == Status.CriticalChance);
            
            var upgradeCriticalDamageStat = PlayerData.GetUpgradeHistory().selectStatTable.FirstOrDefault(status =>
                status.selectStatName == Status.CriticalDamage);

            var skillCriticalChanceEnchant = characterSkill.skillEnchantTables
                .FirstOrDefault(e => e.enchantEffect1 == Status.CriticalChance);
            
            var skillCriticalDamageEnchant = characterSkill.skillEnchantTables
                .FirstOrDefault(e => e.enchantEffect1 == Status.CriticalDamage);

            var shopCriticalChanceStats = PlayerData.GetCharacterStats().Where(status =>
                status.upgradeName == Status.CriticalChance);
            var shopCriticalDamageStats = PlayerData.GetCharacterStats().Where(status => 
                status.upgradeName == Status.CriticalDamage);
            
            var upgradeCriticalChance = (upgradeCriticalChanceStat?.currentUpgradeCount ?? 0) *
                                        (upgradeCriticalChanceStat?.selectStatRateValue ?? 0);
            
            var upgradeCriticalDamage = (upgradeCriticalDamageStat?.currentUpgradeCount ?? 0) *
                                        (upgradeCriticalDamageStat?.selectStatRateValue ?? 0);

            var skillCriticalChance = (skillCriticalChanceEnchant?.enchantEffectValue1 ?? 0) * 
                                      (skillCriticalChanceEnchant?.currentCount ?? 0);

            var skillCriticalDamage = (skillCriticalDamageEnchant?.enchantEffectValue1 ?? 0) *
                                      (skillCriticalDamageEnchant?.currentCount ?? 0);

            var shopCriticalChance = shopCriticalChanceStats.Sum(stats => stats.addStatValue);
            var shopCriticalDamage = shopCriticalDamageStats.Sum(stats => stats.addStatValue);

            var resultCriticalChance = upgradeCriticalChance + skillCriticalChance + shopCriticalChance +
                                       characterBaseData.criticalPercent;
            var resultCriticalDamage = upgradeCriticalDamage + skillCriticalDamage + shopCriticalDamage +
                                       characterBaseData.criticalDamage + characterSkill.skillTable.criticalDamageRate;

            resultCriticalChance = Math.Max(0, resultCriticalChance);
            resultCriticalDamage = Math.Max(0, resultCriticalDamage);
            
            bool isCriticalHit = UnityEngine.Random.value * 100 < resultCriticalChance;
            
            if (isCriticalHit)
            {
                baseDamage += (baseDamage * resultCriticalDamage);
            }

        }

        return baseDamage;
    }

    public float PlayerAttackSpeed(int skillID)
    {
        var characterSkill = PlayerData.GetCharacterSkills()[skillID];
        var characterBaseData = GameManager.Instance.PlayerMove.characterData;        
        
        var upgradeAttackSpeedStat = PlayerData.GetUpgradeHistory().selectStatTable.FirstOrDefault(status =>
            status.selectStatName == Status.AttackSpeedRate);

        var skillAttackSpeedEnchant = characterSkill.skillEnchantTables
            .FirstOrDefault(e => e.enchantEffect1 == Status.AttackSpeedRate);
        var shopAttackSpeedStats = PlayerData.GetCharacterStats().Where(status =>
            status.upgradeName == Status.AttackSpeed);

        var upgradeAttackSpeedRate = (upgradeAttackSpeedStat?.currentUpgradeCount ?? 0) *
                                 (upgradeAttackSpeedStat?.selectStatRateValue ?? 0);
        
        var skillAttackSpeedRate = (skillAttackSpeedEnchant?.enchantEffectValue1 ?? 0) * 
                                  (skillAttackSpeedEnchant?.currentCount ?? 0);
        
        var shopAttackSpeed = shopAttackSpeedStats.Sum(stats => stats.addStatValue);

        return (characterBaseData.attackSpeed + shopAttackSpeed) +
               ((characterBaseData.attackSpeed + shopAttackSpeed) * (characterSkill.skillTable.skillSpeedRate + upgradeAttackSpeedRate + skillAttackSpeedRate));
    }
}
