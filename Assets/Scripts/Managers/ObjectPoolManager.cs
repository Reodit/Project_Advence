using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections.Generic;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    protected override void Awake()
    {
        _poolDictionary = new Dictionary<string, Dictionary<string, ObjectPool<GameObject>>>();
    }

    private Dictionary<string, Dictionary<string, ObjectPool<GameObject>>> _poolDictionary;

    public void CreatePool(string poolName, List<string> prefabPaths, int initialSize = 10, int maxSize = 100)
    {
        if (!_poolDictionary.ContainsKey(poolName))
        {
            _poolDictionary[poolName] = new Dictionary<string, ObjectPool<GameObject>>();
        }

        foreach (var prefabPath in prefabPaths)
        {
            if (!_poolDictionary[poolName].ContainsKey(prefabPath))
            {
                _poolDictionary[poolName][prefabPath] = new ObjectPool<GameObject>(
                    createFunc: () =>
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>(prefabPath), this.transform);
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
        if (_poolDictionary == null)
        {
            return;
        }
        
        foreach (var poolByPrefab in _poolDictionary!.Values)
        {
            foreach (var pool in poolByPrefab!.Values)
            {
                pool.Clear();
            }
        }

        _poolDictionary.Clear();
    }
    
    public GameObject SpawnFromPool(string poolName, string prefabPath, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
    {
        if (!_poolDictionary.ContainsKey(poolName) || !_poolDictionary[poolName].ContainsKey(prefabPath))
        {
            Debug.LogWarning($"Pool with tag {poolName} and prefab {prefabPath} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[poolName][prefabPath].Get();

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

        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }

    public void ReturnToPool(string poolName, string prefabPath, GameObject obj)
    {
        if (!obj.activeInHierarchy)
        {
            Debug.LogWarning($"Object {obj.name} is already released to the pool.");
            return;
        }
        
        if (!_poolDictionary.ContainsKey(poolName) || !_poolDictionary[poolName].ContainsKey(prefabPath))
        {
            Debug.LogWarning($"Pool with tag {poolName} and prefab {prefabPath} doesn't exist.");
            return;
        }

        obj.SetActive(false);
        _poolDictionary[poolName][prefabPath].Release(obj);
    }
}
