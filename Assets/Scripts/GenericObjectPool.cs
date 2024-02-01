using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPooledObject
{
    /// <summary>
    /// 오브젝트가 인스턴싱 될 때 호출
    /// </summary>
    void OnObjectInstantiate();

    /// <summary>
    /// 오브젝트가 풀에서 가져올 때 호출 
    /// </summary>
    void OnObjectSpawn(); 
    
    /// <summary>
    /// 오브젝트가 풀에 반환될 때 호출
    /// </summary>
    void OnObjectReturn(); 

    /// <summary>
    /// 오브젝트가 파괴될 때 호출
    /// </summary>
    void OnObjectDestroy();
}

public class GenericObjectPool<TComponent> where TComponent : Component, IPooledObject
{
    private ObjectPool<TComponent> _pool;
    
    public GenericObjectPool(TComponent prefab, int initialSize = 10, int maxSize = 100)
    {
        _pool = new ObjectPool<TComponent>(
            createFunc: () => {
                var obj = Object.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                obj.OnObjectInstantiate();
                return obj;
            },
            actionOnGet: (obj) => {
                obj.gameObject.SetActive(true);
                obj.OnObjectSpawn();
            },
            actionOnRelease: (obj) => {
                obj.gameObject.SetActive(false);
                obj.OnObjectReturn();
            },
            actionOnDestroy: obj =>
            {
                obj.OnObjectDestroy();
                Object.Destroy(obj);
            },
            maxSize: maxSize
        );

        for (int i = 0; i < initialSize; i++)
        {
            var obj = _pool.Get();
            _pool.Release(obj);
        }
    }
    
    public TComponent Get()
    {
        return _pool.Get();
    }

    public void Release(TComponent obj)
    {
        _pool.Release(obj);
    }
}
