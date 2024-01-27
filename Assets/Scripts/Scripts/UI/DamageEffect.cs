using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class DamageEffect : MonoBehaviour
    {
        public abstract void Init();
        public abstract void RestoreDefaults();
        public abstract IEnumerator DisplayDamage(List<string> damages);

        public abstract void PositionUpdate(Vector3 monsterTransform);
    }
}
