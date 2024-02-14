using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeDebugger : MonoBehaviour
{
    private Dictionary<string, CharacterSkill> _skillDict = new Dictionary<string, CharacterSkill>();
    private Dictionary<int, SkillEnchantTable> _enchantDict = new Dictionary<int, SkillEnchantTable>();

    private SkillTable _lightningBolt;
    private SkillTable _fireball;
    private SkillTable _blueLaser;
    private const int DAMAGE = 14000;
    private const int ATTACK_RATE = 14001;
    private const int RANGE = 14002;
    private const int SPEED = 14003;
    private const int SLASH_PROJECTILE = 14004;
    private const int FRONT_PROJECTILE = 14005;

    
    public void Spawn(int skillIndex)
    {
        EnchantDictInit();

        switch (skillIndex)
        {
            case 10001:
                SpawnFireball();
                break;
            case 10002:
                SpawnLightningBolt();
                break;
            case 10003:
                SpawnBlueLaser();
                break;
        }
    }

    public void UpgradeDamage(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                UpgradeFireballDamage();
                break;
            case 10002:
                UpgradeLightningboltDamage();
                break;
            case 10003:
                UpgradeBluelaserDamage();
                break;
        }
    }

    public void UpgradeAttackRate(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                UpgradeFireballAttackRate();
                break;
            case 10002:
                UpgradeLightningboltAttackRate();
                break;
            case 10003:
                UpgradeBluelaserAttackRate();
                break;
        }
    }

    public void UpgradeRange(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                UpgradeFireballRange();
                break;
            case 10002:
                UpgradeLightningBoltRange();
                break;
            case 10003:
                UpgradeBlueLaserRange();
                break;
        }
    }
    
    public void UpgradeSpeed(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                UpgradeFireballSpeed();
                break;
            case 10002:
                UpgradeLightningBoltSpeed();
                break;
            case 10003:
                UpgradeBlueLaserSpeed();
                break;
        }
    }

    public void UpgradeFront(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                UpgradeFrontFireball();
                break;
            case 10002:
                UpgradeFrontLightningBolt();
                break;
            case 10003:
                UpgradeFrontBlueLaser();
                break;
        }
    }

    public void UpgradeSlash(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                UpgradeSlashFireball();
                break;
            case 10002:
                UpgradeSlashLightningBolt();
                break;
            case 10003:
                UpgradeSlashBlueLaser();
                break;
        }
    }

    public bool IsSpawned(int skillIndex)
    {
        switch (skillIndex)
        {
            case 10001:
                return IsFireballSpawned();
            case 10002:
                return IsLightningboltSpawned();
            case 10003:
                return IsBuleLaserSpawned();
        }

        return false;
    }

    private bool IsBuleLaserSpawned()
    {
        return _blueLaser != null;
    }

    private bool IsFireballSpawned()
    {
        return _fireball != null;
    }

    private bool IsLightningboltSpawned()
    {
        return _lightningBolt != null;
    }

    private void SpawnFireball()
    {
        if (_fireball == null)
        {
            _fireball = Datas.GameData.DTSkillData[10001];
            SpawnBullet(_fireball);
        }
    }

    private void SpawnLightningBolt()
    {
        if (_lightningBolt == null)
        {
            _lightningBolt = Datas.GameData.DTSkillData[10002];
            SpawnBullet(_lightningBolt);
        }
    }

    private void SpawnBlueLaser()
    {
        if (_blueLaser == null)
        {
            _blueLaser = Datas.GameData.DTSkillData[10003];
            SpawnBullet(_blueLaser);
        }
    }

    private void UpgradeFireballDamage()
    {
        SkillManager.instance.AddSkillEnchant(_fireball.name, Datas.GameData.DTSkillEnchantData[DAMAGE]);
    }

    private void UpgradeLightningboltDamage()
    {
        SkillManager.instance.AddSkillEnchant(_lightningBolt.name, Datas.GameData.DTSkillEnchantData[DAMAGE]);
    }

    private void UpgradeBluelaserDamage()
    {
        SkillManager.instance.AddSkillEnchant(_blueLaser.name, Datas.GameData.DTSkillEnchantData[DAMAGE]);
    }

    private void UpgradeFireballAttackRate()
    {
        SkillManager.instance.AddSkillEnchant(_fireball.name, Datas.GameData.DTSkillEnchantData[ATTACK_RATE]);
    }

    private void UpgradeLightningboltAttackRate()
    {
        SkillManager.instance.AddSkillEnchant(_lightningBolt.name, Datas.GameData.DTSkillEnchantData[ATTACK_RATE]);
    }

    private void UpgradeBluelaserAttackRate()
    {
        SkillManager.instance.AddSkillEnchant(_blueLaser.name, Datas.GameData.DTSkillEnchantData[ATTACK_RATE]);
    }

    private void UpgradeFireballRange()
    {
        SkillManager.instance.AddSkillEnchant(_fireball.name, Datas.GameData.DTSkillEnchantData[RANGE]);
    }

    private void UpgradeLightningBoltRange()
    {
        SkillManager.instance.AddSkillEnchant(_lightningBolt.name, Datas.GameData.DTSkillEnchantData[RANGE]);
    }
    private void UpgradeBlueLaserRange()
    {
        SkillManager.instance.AddSkillEnchant(_blueLaser.name, Datas.GameData.DTSkillEnchantData[RANGE]);
    }

    private void UpgradeFireballSpeed()
    {
        SkillManager.instance.AddSkillEnchant(_fireball.name, Datas.GameData.DTSkillEnchantData[SPEED]);
    }

    private void UpgradeLightningBoltSpeed()
    {
        SkillManager.instance.AddSkillEnchant(_lightningBolt.name, Datas.GameData.DTSkillEnchantData[SPEED]);
    }

    private void UpgradeBlueLaserSpeed()
    {
        SkillManager.instance.AddSkillEnchant(_blueLaser.name, Datas.GameData.DTSkillEnchantData[SPEED]);
    }

    private void UpgradeFrontFireball()
    {
        SkillManager.instance.AddSkillEnchant(_fireball.name, Datas.GameData.DTSkillEnchantData[FRONT_PROJECTILE]);
    }

    private void UpgradeFrontLightningBolt()
    {
        SkillManager.instance.AddSkillEnchant(_lightningBolt.name, Datas.GameData.DTSkillEnchantData[FRONT_PROJECTILE]);
    }

    private void UpgradeFrontBlueLaser()
    {
        SkillManager.instance.AddSkillEnchant(_blueLaser.name, Datas.GameData.DTSkillEnchantData[FRONT_PROJECTILE]);
    }

    private void UpgradeSlashFireball()
    {
        SkillManager.instance.AddSkillEnchant(_fireball.name, Datas.GameData.DTSkillEnchantData[SLASH_PROJECTILE]);
    }

    private void UpgradeSlashLightningBolt()
    {
        SkillManager.instance.AddSkillEnchant(_lightningBolt.name, Datas.GameData.DTSkillEnchantData[SLASH_PROJECTILE]);
    }

    private void UpgradeSlashBlueLaser()
    {
        SkillManager.instance.AddSkillEnchant(_blueLaser.name, Datas.GameData.DTSkillEnchantData[SLASH_PROJECTILE]);
    }

    


    private void SpawnBullet(SkillTable skill)
    {
        if (_skillDict.Count == 0)
        {
            _skillDict = SkillManager.instance.playerSkills;
        }

        SkillManager.instance.AddPlayerSkill(skill);
    }

    private void EnchantDictInit()
    {
        if (_enchantDict.Count == 0)
            _enchantDict = Datas.GameData.DTSkillEnchantData;
    }

   
}
