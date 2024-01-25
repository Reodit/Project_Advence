using UnityEngine;
using UnityEditor;

public class ExcelReaderWindow : EditorWindow
{
    [MenuItem("Excel/Excel Reader")]
    public static void ShowWindow()
    {
        GetWindow<ExcelReaderWindow>("Excel Reader");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Read All Excel Files in DataSheets Folder"))
        {
            ReadAllExcelFilesWithProgress();
        }
    }

    private void ReadAllExcelFilesWithProgress()
    {
    }
}
