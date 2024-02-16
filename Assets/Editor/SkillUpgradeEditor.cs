using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillUpgradeDebugger))]
public class SkillUpgradeEditor : Editor
{
    private SkillUpgradeDebugger _debugger;

    private GUIStyle _center;

    private Dictionary<string, int> _skillInfoDict = new Dictionary<string, int>();

    int _selected;

    string[] _tempNameArr;

    private void OnEnable()
    {
        _debugger = (SkillUpgradeDebugger)target;
        _center = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        _center.normal.textColor = Color.green;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ShowEditorInGame();
    }

    private void ShowEditorInGame()
    {
        UpdateSpawnInfo();

        if (_skillInfoDict.Count > 0)
        {
            int skillIndex = _skillInfoDict[_tempNameArr[_selected]];

            _selected = EditorGUILayout.Popup("ChooseSkill", _selected, _tempNameArr, EditorStyles.popup);

            GUILayout.Label("Spawn", _center);

            if (GUILayout.Button("Spawn"))
            {
                _debugger.Spawn(skillIndex);
            }

            GUILayout.Label("Upgrade", _center);

            if (GUILayout.Button("Damage"))
            {
                if (!_debugger.IsSpawned(skillIndex))
                    return;

                _debugger.UpgradeDamage(skillIndex);
            }

            if (GUILayout.Button("AttackRate"))
            {
                if (!_debugger.IsSpawned(skillIndex))
                    return;

                _debugger.UpgradeAttackRate(skillIndex);
            }

            if (GUILayout.Button("Range"))
            {
                if (!_debugger.IsSpawned(skillIndex))
                    return;

                _debugger.UpgradeRange(skillIndex);
            }

            if (GUILayout.Button("Speed"))
            {
                if (!_debugger.IsSpawned(skillIndex))
                    return;

                _debugger.UpgradeSpeed(skillIndex);
            }

            if (GUILayout.Button("Front"))
            {
                if (!_debugger.IsSpawned(skillIndex))
                    return;

                _debugger.UpgradeFront(skillIndex);
            }

            if (GUILayout.Button("Slash"))
            {
                if (!_debugger.IsSpawned(skillIndex))
                    return;

                _debugger.UpgradeSlash(skillIndex);
            }
        }
    }

    private void UpdateSpawnInfo()
    {
        if (Datas.GameData.DTSkillData.Count != 0)
        {
            _tempNameArr = new string[Datas.GameData.DTSkillData.Count];

            int i = 0;
            foreach (var skill in Datas.GameData.DTSkillData.Values)
            {
                if (!_skillInfoDict.ContainsKey(skill.name))
                {
                    _skillInfoDict.Add(skill.name, skill.index);
                }

                i++;
            }

            Array.Copy(_skillInfoDict.Keys.ToArray(), _tempNameArr, _skillInfoDict.Count);
        }
    }
}
