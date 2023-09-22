using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal.VR;

public class TechTreeEditor : EditorWindow
{
    [MenuItem("Tools/TechTreeEditor")]
    static void Init()
    {
        TechTreeEditor window = (TechTreeEditor)EditorWindow.GetWindow(typeof(TechTreeEditor));
        window.Show();
    }

    private TextAsset _techTreeFile = null;
    private const string _techTreeFolderPath = "Assets/TechTreee/";


    private void OnGUI()
    {
        _techTreeFile = (TextAsset)EditorGUILayout.ObjectField(_techTreeFile, typeof(TextAsset), false);

        if (GUILayout.Button("Create"))
        {
            Debug.Log("Create");
        }
    }
}
