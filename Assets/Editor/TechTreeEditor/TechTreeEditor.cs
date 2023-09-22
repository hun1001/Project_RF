using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Stage;
using UnityEditorInternal;

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

        Tank tank = null;
        tank = (Tank)EditorGUILayout.ObjectField(tank, typeof(Tank), false);

        if (GUILayout.Button("Create"))
        {

        }
    }


    private void EditMode()
    {
        _techTreeFile = (TextAsset)EditorGUILayout.ObjectField(_techTreeFile, typeof(TextAsset), false);

        GUILayout.TextArea("Developing");
    }
}
