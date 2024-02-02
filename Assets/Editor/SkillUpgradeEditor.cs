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

        GUILayout.Label("Upgrade Front", _center);

        if (GUILayout.Button("UpgradeFrontFireball"))
        {
            _debugger.UpgradeFrontFireball();
        }

        if (GUILayout.Button("UpgradeFrontLightningBolt"))
        {
            _debugger.UpgradeFrontLightningBolt();
        }

        if (GUILayout.Button("UpgradeFrontBlueLaser"))
        {
            _debugger.UpgradeFrontBlueLaser();
        }

        GUILayout.Label("Upgrade Slash", _center);

        if (GUILayout.Button("UpgradeSlashFireball"))
        {
            _debugger.UpgradeSlashFireball();
        }

        if (GUILayout.Button("UpgradeSlashLightningBolt"))
        {
            _debugger.UpgradeSlashLightningBolt();
        }

        if (GUILayout.Button("UpgradeSlashBlueLaser"))
        {
            _debugger.UpgradeSlashBlueLaser();
        }

    }
}
