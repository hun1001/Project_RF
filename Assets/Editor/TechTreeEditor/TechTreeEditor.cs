using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TechTreeEditor : EditorWindow
{
    private TechTreeSO _techTreeSO;
    private Tank[] _techTreeNodes;
    private int _techTreeLength;

    [MenuItem("Window/TechTreeEditor")]
    static void Init()
    {
        TechTreeEditor window = (TechTreeEditor)EditorWindow.GetWindow(typeof(TechTreeEditor));
        window.Show();
    }

    void OnGUI()
    {
        _techTreeSO = (TechTreeSO)EditorGUILayout.ObjectField("Tech Tree", _techTreeSO, typeof(TechTreeSO), false);

        _techTreeLength = EditorGUILayout.IntField("Tech Tree Length", _techTreeLength);

        if (_techTreeLength != _techTreeNodes.Length)
        {
            _techTreeNodes = new Tank[_techTreeLength];
        }

        for (int i = 0; i < _techTreeNodes.Length; ++i)
        {
            _techTreeNodes[i] = (Tank)EditorGUILayout.ObjectField("Tech Tree Node " + i, _techTreeNodes[i], typeof(Tank), true);
        }
    }
}
