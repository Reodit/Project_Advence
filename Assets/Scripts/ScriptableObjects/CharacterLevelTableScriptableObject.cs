using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterLevelTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<CharacterLevelTable> characterLevelTableList;
}