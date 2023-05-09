using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TechTreeTool : EditorWindow
{
    [MenuItem("Tools/TechTreeTool")]
    static void Init()
    {
        TechTreeTool window = (TechTreeTool)EditorWindow.GetWindow(typeof(TechTreeTool));
        window.Show();
    }

    private void OnGUI()
    {

    }
}
