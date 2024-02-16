using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeDebugger : MonoBehaviour
{
    private Dictionary<int, SkillTable> _skillDict = new Dictionary<int, SkillTable>();

    private const int DAMAGE = 14000;
    private const int ATTACK_RATE = 14001;
    private const int RANGE = 14002;
    private const int SPEED = 14003;
    private const int SLASH_PROJECTILE = 14004;
    private const int FRONT_PROJECTILE = 14005;

    private PlayerMove _player;
    public bool isLevelUpOn = true;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMove>();
        _player.isLevelUpOn = isLevelUpOn;
    }

    public void Spawn(int skillIndex)
    {
        if (!_skillDict.ContainsKey(skillIndex))
            _skillDict.Add(skillIndex, Datas.GameData.DTSkillData[skillIndex]);

        SkillManager.instance.AddPlayerSkill(_skillDict[skillIndex]);
    }

    public void UpgradeDamage(int skillIndex)
    {
        SkillManager.instance.AddSkillEnchant(skillIndex, Datas.GameData.DTSkillEnchantData[DAMAGE]);
    }

    public void UpgradeAttackRate(int skillIndex)
    {
        SkillManager.instance.AddSkillEnchant(skillIndex, Datas.GameData.DTSkillEnchantData[ATTACK_RATE]);
    }

    public void UpgradeRange(int skillIndex)
    {
        SkillManager.instance.AddSkillEnchant(skillIndex, Datas.GameData.DTSkillEnchantData[RANGE]);
    }

    public void UpgradeSpeed(int skillIndex)
    {
        SkillManager.instance.AddSkillEnchant(skillIndex, Datas.GameData.DTSkillEnchantData[SPEED]);
    }

    public void UpgradeFront(int skillIndex)
    {
        SkillManager.instance.AddSkillEnchant(skillIndex, Datas.GameData.DTSkillEnchantData[FRONT_PROJECTILE]);
    }

    public void UpgradeSlash(int skillIndex)
    {
        SkillManager.instance.AddSkillEnchant(skillIndex, Datas.GameData.DTSkillEnchantData[SLASH_PROJECTILE]);
    }

    public bool IsSpawned(int skillIndex)
    {
        return _skillDict.ContainsKey(skillIndex);
    }
}
