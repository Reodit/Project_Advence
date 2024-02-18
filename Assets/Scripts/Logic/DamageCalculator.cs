using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Datas;
using Enums;

public static class DamageCalculator
{
    public static float PlayerResultSkillDamage(int skillID, bool isCriticalApply = true)
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

    public static float PlayerAttackSpeed(int skillID)
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
    
        Debug.Log($"baseAttackSpeed : {(characterBaseData.attackSpeed + shopAttackSpeed) + ((characterBaseData.attackSpeed + shopAttackSpeed) * (characterSkill.skillTable.skillSpeedRate + upgradeAttackSpeedRate + skillAttackSpeedRate))}     " +
                  $"skillId : {skillID}");

        
        return (characterBaseData.attackSpeed + shopAttackSpeed) +
               ((characterBaseData.attackSpeed + shopAttackSpeed) * (characterSkill.skillTable.skillSpeedRate + upgradeAttackSpeedRate + skillAttackSpeedRate));
    }
}
