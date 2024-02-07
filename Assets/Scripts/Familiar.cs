using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Familiar : MonoBehaviour
{
    [HideInInspector] public SkillTable SkillData;
    [HideInInspector] public FamiliarData FamiliarSkillData;
    protected float attackDamage;
    public float spawnCoolTime;
    [SerializeField] protected BulletController bulletController;
    
    [SerializeField] private Familiar scale;
    public string familiarSpawnCoolTimeID;

    protected virtual void Update()
    {
        if (CheckDestroyCondition())
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        spawnCoolTime = SkillData.skillSpeedRate;
        attackDamage = SkillData.skillDamageRate * 15;
//        familiarSpawnCoolTimeID = "familiarSpawn_" + gameObject.GetInstanceID();
//        TimeManager.Instance.RegisterCoolTime(familiarSpawnCoolTimeID, spawnCoolTime);
    }
    
    public virtual void HitMonster(Monster monster)
    {
        
    }
    
    protected virtual bool CheckDestroyCondition()
    {
        return false;
    }
}
