using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillEnchantTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<SkillEnchantTable> SkillEnchantTables;
}
