using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelectStatTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<SelectStatTable> SelectStatTableList;
}
