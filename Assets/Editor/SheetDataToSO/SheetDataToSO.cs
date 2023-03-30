using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SheetDataToSO : EditorWindow
{
    [MenuItem("Window/SheetDataToSO")]
    static void Init()
    {
        SheetDataToSO window = (SheetDataToSO)EditorWindow.GetWindow(typeof(SheetDataToSO));
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Load"))
        {
            Debug.Log("Load Data");
        }
    }
}
