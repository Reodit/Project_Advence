using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnDebugger : MonoBehaviour
{
    private BulletController _bulletSpawner;

    private float _angle = 60f;

    public float Angle
    {
        get 
        {
            if (!_bulletSpawner)
            {
                return _angle;
            }

            return _bulletSpawner.Angle; 
        }
        set
        {
            _angle = value;
            if (!_bulletSpawner)
            {
                _bulletSpawner = FindObjectType<BulletController>();
            }
            else
            {
#if UNITY_EDITOR
               _bulletSpawner.SetLowerBulletRotation(value);
#endif
            }
        }
    }

    private T FindObjectType<T>() where T : MonoBehaviour
    {
        return FindObjectOfType<T>();
    }
}
