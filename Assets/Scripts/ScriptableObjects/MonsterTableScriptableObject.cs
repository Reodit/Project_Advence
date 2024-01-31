using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<MonsterTable> MonsterTables;
}
