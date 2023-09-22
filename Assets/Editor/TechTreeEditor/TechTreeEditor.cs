using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Addressable;

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

    private TechTreeEditorMode _mode = TechTreeEditorMode.Create;
    private CountryType _countryType = CountryType.None;

    private TechTree _techTree = null;
    private List<List<Tank>> _tanks = new List<List<Tank>>();

    private void OnEnable()
    {
        _tanks.Add(new List<Tank>());
        _tanks[0].Add(null);
    }

    private void OnGUI()
    {
        _mode = (TechTreeEditorMode)EditorGUILayout.EnumPopup(_mode);

        switch (_mode)
        {
            case TechTreeEditorMode.Create:
                CreateMode();
                break;
            case TechTreeEditorMode.Edit:
                EditMode();
                break;
        }
    }

    private void CreateMode()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup(_countryType);


        for (int i = 0;i < _tanks.Count;i++)
        {
            GUILayout.BeginHorizontal();
            for(int j = 0;j < _tanks[i].Count;j++)
            {
                GUILayout.BeginVertical();
                _tanks[i][j] = (Tank)EditorGUILayout.ObjectField(_tanks[i][j], typeof(Tank), false);
                if(GUILayout.Button("Add"))
                {
                    _tanks.Add(new List<Tank>());
                    _tanks[i + 1].Add(null);
                }
                GUILayout.EndVertical();
            }

            if(GUILayout.Button("Add"))
            {
                _tanks[i].Add(null);
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Create"))
        {

        }
    }


    private void EditMode()
    {
        _techTreeFile = (TextAsset)EditorGUILayout.ObjectField(_techTreeFile, typeof(TextAsset), false);

        GUILayout.Label("Developing");
    }
}
