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

    private Dictionary<string, Dictionary<string, IObjectPool<GameObject>>> _poolDictionary;

    private void Start()
    {
        _poolDictionary = new Dictionary<string, Dictionary<string, IObjectPool<GameObject>>>();
    }

    public void CreatePool(string poolName, List<GameObject> prefabs, int initialSize = 10, int maxSize = 100)
    {
        if (!_poolDictionary.ContainsKey(poolName))
        {
            _poolDictionary[poolName] = new Dictionary<string, IObjectPool<GameObject>>();
        }

        foreach (var prefab in prefabs)
        {
            if (!_poolDictionary[poolName].ContainsKey(prefab.name))
            {
                _poolDictionary[poolName][prefab.name] = new ObjectPool<GameObject>(
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
        foreach (var poolByPrefab in _poolDictionary.Values)
        {
            foreach (var pool in poolByPrefab.Values)
            {
                pool.Clear();
            }
        }

        _poolDictionary.Clear();
    }
    
    public GameObject SpawnFromPool(string poolName, string prefabName, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
    {
        if (!_poolDictionary.ContainsKey(poolName) || !_poolDictionary[poolName].ContainsKey(prefabName))
        {
            Debug.LogWarning($"Pool with tag {poolName} and prefab {prefabName} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[poolName][prefabName].Get();
        
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

    public void ReturnToPool(string poolName, string prefabName, GameObject obj)
    {
        if (!_poolDictionary.ContainsKey(poolName) || !_poolDictionary[poolName].ContainsKey(prefabName))
        {
            Debug.LogWarning($"Pool with tag {poolName} and prefab {prefabName} doesn't exist.");
            return;
        }

        _poolDictionary[poolName][prefabName].Release(obj);
    }
}
