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

    private const string _techTreeFolderPath = "Assets/TechTreee/";

    private TextAsset _techTreeFile = null;
    private TechTree _techTree = null;

    private TechTreeEditorMode _mode = TechTreeEditorMode.None;
    private CountryType _countryType = CountryType.None;


    private void OnGUI()
    {
        SelectMode();

        switch (_mode)
        {
            case TechTreeEditorMode.None:
                NoneMode();
                break;
            case TechTreeEditorMode.Create:
                CreateMode();
                break;
            case TechTreeEditorMode.Edit:
                EditMode();
                break;
        }
    }

    private void SelectMode()
    {
        TechTreeEditorMode newMode = (TechTreeEditorMode)EditorGUILayout.EnumPopup(_mode);

        if(newMode != _mode)
        {
            _mode = newMode;
            OnModeChanged(_mode);
        }
    }

    private void OnModeChanged(TechTreeEditorMode changedMode)
    {
        switch (changedMode)
        {
            case TechTreeEditorMode.None:
                OnNoneModeChaged();
                break;
            case TechTreeEditorMode.Create:
                OnCreateModeChanged();
                break;
            case TechTreeEditorMode.Edit:
                OnEditModeChanged();
                break;
        }
    }

    private void OnNoneModeChaged()
    {
        _techTreeFile = null;
        _techTree = null;
    }

    private void OnCreateModeChanged()
    {
        _techTreeFile = null;
        _techTree = new TechTree();

        var node = _techTree.Root;

        for(int i = 0; i < 3; i++)
        {
            node.tankAddress = "T-34";
            var newNode = new TechTreeNode();
            node._child = (newNode);
            node = newNode;
        }

        node.tankAddress = "T-34";
    }

    private void OnEditModeChanged()
    {
        _techTree = null;
        _techTreeFile = null;
    }

    private void NoneMode()
    {
        GUILayout.Label("Please Select Mode");
    }

    private void CreateMode()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup(_countryType);

        var _node = _techTree.Root;

        GUILayout.BeginHorizontal();

        while(_node != null)
        {
            Tank tank = null;

            if(_node.tankAddress != "")
            {
                tank = AddressablesManager.Instance.GetResource<GameObject>(_node.tankAddress).GetComponent<Tank>();
            }

            tank = EditorGUILayout.ObjectField(tank, typeof(Tank), false) as Tank;
            _node.tankAddress = tank.ID;

            _node = _node._child;
        }

        GUILayout.EndHorizontal();


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
