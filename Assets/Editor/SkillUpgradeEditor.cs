using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillUpgradeDebugger))]
public class SkillUpgradeEditor : Editor
{
    private SkillUpgradeDebugger _debugger;

    private GUIStyle _center;

    private void OnEnable()
    {
        _debugger = (SkillUpgradeDebugger)target;
        _center = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        _center.normal.textColor = Color.green;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();



        GUILayout.Label("Spawn", _center);

        if (GUILayout.Button("SpawnFireball"))
        {
            _debugger.SpawnFireball();
        }

        if (GUILayout.Button("SpawnLightningBolt"))
        {
            _debugger.SpawnLightningBolt();
        }

        if (GUILayout.Button("SpawnBlueLaser"))
        {
            _debugger.SpawnBlueLaser();
        }

        GUILayout.Label("Upgrade Straight", _center);

        if (GUILayout.Button("UpgradeStraightFireball"))
        {
            _debugger.UpgradeStraightFireball();
        }

        if (GUILayout.Button("UpgradeStraightLightningBolt"))
        {
            _debugger.UpgradeStraightLightningBolt();
        }

        if (GUILayout.Button("UpgradeStraightBlueLaser"))
        {
            _debugger.UpgradeStraightBlueLaser();
        }

        GUILayout.Label("Upgrade Cross", _center);

        if (GUILayout.Button("UpgradeCrossFireball"))
        {
            _debugger.UpgradeCrossFireball();
        }

        if (GUILayout.Button("UpgradeCrossLightningBolt"))
        {
            _debugger.UpgradeStraightLightningBolt();
        }

        if (GUILayout.Button("UpgradeCrossBlueLaser"))
        {
            _debugger.UpgradeStraightBlueLaser();
        }

    }
}
