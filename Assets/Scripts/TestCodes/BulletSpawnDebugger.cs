using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnDebugger : MonoBehaviour
{
    private BulletSpawner _bulletSpawner;

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
                _bulletSpawner = FindBulletSpawner<BulletSpawner>();
            }
            else
            {
               _bulletSpawner.SetLowerBulletRotation(value);
            }
        }
    }

    private T FindBulletSpawner<T>() where T : MonoBehaviour
    {
        return FindObjectOfType<T>();
    }
}
