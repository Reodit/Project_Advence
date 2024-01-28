using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatternTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<PatternTable> Patterns;
}
