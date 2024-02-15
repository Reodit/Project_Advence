using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datas;
using Enums;

public static class DamageCalculator
{
    public static int ResultSkillDamage(int skillID)
    {
        var characterSkill = PlayerData.GetCharacterSkills()[skillID];

        switch (characterSkill.skillTable.type)
        {
            case SkillType.Creature:
                break;
            default:
                break;
        }
        
        return 0;
    }
}
