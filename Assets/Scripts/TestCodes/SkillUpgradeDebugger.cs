using NPOI.OpenXmlFormats.Dml.Diagram;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillUpgradeDebugger : MonoBehaviour
{
    private Dictionary<string, CharacterSkill> _skillDict = new Dictionary<string, CharacterSkill>();
    private Dictionary<int, SkillEnchantTable> _enchantDict = new Dictionary<int, SkillEnchantTable>();

    private SkillTable _lightningBolt;
    private SkillTable _fireball;
    private SkillTable _blueLaser;
    private const int CROSS_PROJECTILE = 14004;
    private const int STRAIGHT_PROJECTILE = 14005;

    

    public void SpawnFireball()
    {
        if (_fireball == null)
        {
            _fireball = Datas.GameData.DTSkillData[10001];
            EnchantDictInit();
            SpawnBullet(_fireball);
        }
    }

    public void SpawnLightningBolt()
    {
        if (_lightningBolt == null)
        {
            _lightningBolt = Datas.GameData.DTSkillData[10002];
            EnchantDictInit();
            SpawnBullet(_lightningBolt);
        }
    }

    public void SpawnBlueLaser()
    {
        if (_blueLaser == null)
        {
            _blueLaser = Datas.GameData.DTSkillData[10003];
            EnchantDictInit();
            SpawnBullet(_blueLaser);
        }
    }

    public void UpgradeStraightFireball()
    {
        if (_fireball == null)
            return;

        _skillDict[_fireball.name].SkillEnchantTables.Add(Datas.GameData.DTSkillEnchantData[STRAIGHT_PROJECTILE]);
    }

    public void UpgradeStraightLightningBolt()
    {
        if (_lightningBolt == null)
            return;

        _skillDict[_lightningBolt.name].SkillEnchantTables.Add(Datas.GameData.DTSkillEnchantData[STRAIGHT_PROJECTILE]);
    }

    public void UpgradeStraightBlueLaser()
    {
        if (_blueLaser == null)
            return;

        _skillDict[_blueLaser.name].SkillEnchantTables.Add(Datas.GameData.DTSkillEnchantData[STRAIGHT_PROJECTILE]);
    }

    public void UpgradeCrossFireball()
    {
        if (_fireball == null)
            return;

        _skillDict[_fireball.name].SkillEnchantTables.Add(Datas.GameData.DTSkillEnchantData[CROSS_PROJECTILE]);
    }

    public void UpgradeCrossLightningBolt()
    {
        if (_lightningBolt == null)    
            return;

        _skillDict[_lightningBolt.name].SkillEnchantTables.Add(Datas.GameData.DTSkillEnchantData[CROSS_PROJECTILE]);
    }

    public void UpgradeCrossBlueLaser()
    {
        if (_blueLaser == null)
            return;

        _skillDict[_blueLaser.name].SkillEnchantTables.Add(Datas.GameData.DTSkillEnchantData[CROSS_PROJECTILE]);
    }

    private void SpawnBullet(SkillTable skill)
    {
        if (_skillDict.Count == 0)
        {
            _skillDict = SkillManager.instance.playerSkills;
        }

        if (!_skillDict.ContainsKey(skill.name))
        {
            _skillDict.Add(skill.name, new CharacterSkill(skill, new List<SkillEnchantTable>()));
        }
    }

    private void EnchantDictInit()
    {
        if (_enchantDict.Count == 0)
            _enchantDict = Datas.GameData.DTSkillEnchantData;
    }

}
