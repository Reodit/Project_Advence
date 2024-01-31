using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterTableScriptableObject : ScriptableObject
{
    [SerializeField] public List<CharacterTable> characterTableList;
}