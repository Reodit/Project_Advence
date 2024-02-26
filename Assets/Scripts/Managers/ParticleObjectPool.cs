using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using UnityEngine;

public class ParticleObjectPool : CustomObjectPool<ParticlePoolObject>
{
    public ParticleObjectPool(ParticlePoolObject prefab, Transform parent, int initialSize = 10, int maxSize = 100) : base(prefab, parent, initialSize, maxSize)
    {
    }

    protected override void CreateNewInstance()
    {
        ParticlePoolObject particle = UnityEngine.Object.Instantiate(prefab, parent);
        particle.gameObject.SetActive(false);
        particle.name = prefab.name;
        pool.Enqueue(particle);
    }
}