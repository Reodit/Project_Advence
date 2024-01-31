using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dictionary<string, CharacterSkill> playerSkills = new Dictionary<string, CharacterSkill>();
    public UpgradeHistory playerUpgradeHistory = new UpgradeHistory(new List<SelectStatTable>());

    private void Awake()
    {
        instance = this;

    }

}
