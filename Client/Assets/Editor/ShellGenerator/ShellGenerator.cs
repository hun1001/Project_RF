using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShellGenerator : EditorWindow
{
    [MenuItem("Tools/ShellGenerator")]
    static void Init()
    {
        ShellGenerator window = (ShellGenerator)EditorWindow.GetWindow(typeof(ShellGenerator));
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate"))
        {

        }
    }
}
