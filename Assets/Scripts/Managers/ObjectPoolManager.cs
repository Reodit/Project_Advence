using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections.Generic;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public static ObjectPoolManager Instance;

    protected override void Awake()
    {
        Instance = this;
    }

    private Dictionary<string, Dictionary<GameObject, IObjectPool<GameObject>>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Dictionary<GameObject, IObjectPool<GameObject>>>();
    }

    public void CreatePool(string poolName, List<GameObject> prefabs, int initialSize = 10, int maxSize = 100)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            poolDictionary[poolName] = new Dictionary<GameObject, IObjectPool<GameObject>>();
        }

        foreach (var prefab in prefabs)
        {
            if (!poolDictionary[poolName].ContainsKey(prefab))
            {
                poolDictionary[poolName][prefab] = new ObjectPool<GameObject>(
                    createFunc: () =>
                    {
                        GameObject obj = Instantiate(prefab);
                        obj.SetActive(false);
                        return obj;
                    },
                    defaultCapacity: initialSize,
                    maxSize: maxSize
                );
            }
        }
    }

    public void ResetPools()
    {
        foreach (var poolByPrefab in poolDictionary.Values)
        {
            foreach (var pool in poolByPrefab.Values)
            {
                pool.Clear();
            }
        }

        poolDictionary.Clear();
    }
    
    public GameObject SpawnFromPool(string poolName, GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(poolName) || !poolDictionary[poolName].ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool with tag {poolName} and prefab {prefab.name} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[poolName][prefab].Get();
        
        if (parent == null)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.localScale = scale;
        }

        else
        {        
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
            objectToSpawn.transform.localScale = scale;
        }

        return objectToSpawn;
    }

    public void ReturnToPool(string poolName, GameObject prefab, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(poolName) || !poolDictionary[poolName].ContainsKey(prefab))
        {
            Debug.LogWarning($"Pool with tag {poolName} and prefab {prefab.name} doesn't exist.");
            return;
        }

        poolDictionary[poolName][prefab].Release(obj);
    }
}
