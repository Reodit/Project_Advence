using System;
using System.Collections;
using System.Collections.Generic;
//using Character.Monster;
using UnityEngine;

public class DamageEffectManager : MonoBehaviour
{
    // 1. 한 씬안에 최대 데미지 이펙트 인스턴스 개수 지정 (100개)
    private const int MaxDamageEffectPrefabInstances = 100;
    
    public static DamageEffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        currentIndex = 0;
        
        CreateDamageEffects();
    }
    
    // 2. 몬스터 타격 시 몬스터 transform + offset 기준으로 최대 개수만큼 instantiate    
    // 데미지 이펙트는 최대 100개로 풀링
    // 3. duration만큼 보여주고 string을 null로 ==> 각자 effect component에서 진행

    [SerializeField] private GameObject damageEffectPrefab;
    private List<GameObject> damageEffectPool;
    private int currentIndex;
    
    private void CreateDamageEffects()
    {
        damageEffectPool = new List<GameObject>(100);

        for (int i = 0; i < MaxDamageEffectPrefabInstances; i++)
        {
            GameObject effectInstance = Instantiate(damageEffectPrefab);
            effectInstance.transform.SetParent(this.transform);
            effectInstance.SetActive(false);
            damageEffectPool.Add(effectInstance);
        }
    }

    public GameObject GetDamageEffect()
    {
        if (currentIndex >= MaxDamageEffectPrefabInstances)
        {
            currentIndex = 0;
        }

        GameObject effectInstance = damageEffectPool[currentIndex];
        currentIndex++;
        
        effectInstance.SetActive(true);

        return effectInstance;
    }

    public void StartDamageDisplayCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }
    
    public void ReturnDamageEffect(GameObject effect)
    {
        effect.SetActive(false);
    }
}
