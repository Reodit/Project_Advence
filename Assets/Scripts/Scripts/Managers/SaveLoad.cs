using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector3 position;
    public Color color;
    public string objectName;
}

public class SaveLoad : MonoBehaviour
{
    public GameObject target;
    public Color targetColor;
    
    public void SaveData(Vector3 position, Color color, string objectName)
    {
        // Vector3 저장
        PlayerPrefs.SetFloat("PositionX", position.x);
        PlayerPrefs.SetFloat("PositionY", position.y);
        PlayerPrefs.SetFloat("PositionZ", position.z);

        // Color 저장
        PlayerPrefs.SetFloat("ColorR", color.r);
        PlayerPrefs.SetFloat("ColorG", color.g);
        PlayerPrefs.SetFloat("ColorB", color.b);
        PlayerPrefs.SetFloat("ColorA", color.a);

        // Object 이름 저장
        PlayerPrefs.SetString("ObjectName", objectName);

        PlayerPrefs.Save();
    }

    public void LoadData(out Vector3 position, out Color color, out string objectName)
    {
        // Vector3 불러오기
        position = new Vector3(
            PlayerPrefs.GetFloat("PositionX"),
            PlayerPrefs.GetFloat("PositionY"),
            PlayerPrefs.GetFloat("PositionZ"));

        // Color 불러오기
        color = new Color(
            PlayerPrefs.GetFloat("ColorR"),
            PlayerPrefs.GetFloat("ColorG"),
            PlayerPrefs.GetFloat("ColorB"),
            PlayerPrefs.GetFloat("ColorA"));

        // Object 이름 불러오기
        objectName = PlayerPrefs.GetString("ObjectName");
    }
    public void SaveDataToJson(Vector3 position, Color color, string objectName)
    {
        SaveData saveData = new SaveData
        {
            position = position,
            color = color,
            objectName = objectName
        };

        string json = JsonUtility.ToJson(saveData);

        string path = Path.Combine(Application.dataPath, "Resources/Json/saveData.json");
        File.WriteAllText(path, json);
    }

    public void OnApplicationQuit()
    {
        SaveData(target.transform.position, targetColor, target.transform.name);

        SaveDataToJson(target.transform.position, targetColor, target.transform.name);
    }
}
