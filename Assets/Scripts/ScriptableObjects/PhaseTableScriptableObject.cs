using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhaseTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<PhaseTable> PhaseTables;
}
