using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SheetDataToSO : EditorWindow
{
    private string _sheetURL = "";

    private string[] _dataType = new string[] { "Tank", "Turret", "Shell" };
    private int _dataTypeIndex = 0;

    [MenuItem("Window/SheetDataToSO")]
    static void Init()
    {
        SheetDataToSO window = (SheetDataToSO)EditorWindow.GetWindow(typeof(SheetDataToSO));
        window.Show();
    }

    void OnGUI()
    {
        _sheetURL = EditorGUILayout.TextField("Sheet URL", _sheetURL);

        _dataTypeIndex = EditorGUILayout.Popup("Data Type", selectedIndex: _dataTypeIndex, displayedOptions: _dataType);

        if (GUILayout.Button("Load"))
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        switch (_dataType[_dataTypeIndex])
        {
            case "Tank":
                Debug.Log("Tank");
                break;
            case "Turret":
                Debug.Log("Turret");
                break;
            case "Shell":
                Debug.Log("Shell");
                break;
            default:
                break;
        }
    }
}
