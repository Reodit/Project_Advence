using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatLevelTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<StatLevelTable> characterLevelTableList;
}