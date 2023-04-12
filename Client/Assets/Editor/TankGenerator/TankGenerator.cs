using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TankGenerator : EditorWindow
{
    [MenuItem("Tools/Tank Generator")]
    static void Init()
    {
        TankGenerator window = (TankGenerator)EditorWindow.GetWindow(typeof(TankGenerator));
        window.Show();
    }
}
