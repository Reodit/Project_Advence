using UnityEngine;

public class ParticleObjectPool : GenericObjectPool<ParticlePoolObject>
{
    public ParticleObjectPool(ParticlePoolObject prefab, int initialSize = 10, int maxSize = 100) : base(prefab, initialSize, maxSize)
    {
    }
}