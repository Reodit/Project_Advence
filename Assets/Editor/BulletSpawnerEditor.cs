using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletSpawnDebugger)), CanEditMultipleObjects]
public class BulletSpawnerEditor : Editor
{
    private BulletSpawnDebugger _bulletSpawnerDebugger;
    private void OnEnable()
    {
        _bulletSpawnerDebugger = (BulletSpawnDebugger)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _bulletSpawnerDebugger.Angle = EditorGUILayout.Slider("LowerBulletRotation", _bulletSpawnerDebugger.Angle, 0f, 90f);

    }
}
