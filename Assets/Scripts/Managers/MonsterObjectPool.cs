using UnityEngine;

public class MonsterObjectPool : CustomObjectPool<Monster>
{
    public MonsterObjectPool(Monster t, Transform parent, int initialSize = 10, int maxSize = 100) : base(t, parent, initialSize, maxSize)
    {
    }
}