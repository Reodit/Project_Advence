using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<SkillTable> SkillTablesList;
}
