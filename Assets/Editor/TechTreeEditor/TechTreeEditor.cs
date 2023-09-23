using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Addressable;
using Unity.Plastic.Newtonsoft.Json;

public class TechTreeEditor : EditorWindow
{
    [MenuItem("Tools/TechTreeEditor")]
    static void Init()
    {
        TechTreeEditor window = (TechTreeEditor)EditorWindow.GetWindow(typeof(TechTreeEditor));
        window.Show();
    }

    private readonly string _techTreeFolderPath = Application.dataPath + "/TechTreeData/";

    private TextAsset _techTreeFile = null;
    private TechTree _techTree = null;

    private TechTreeEditorMode _mode = TechTreeEditorMode.None;
    private CountryType _countryType = CountryType.None;

    private Vector2 _scrollPosition = Vector2.zero;


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

        node.upChildren.Add(new TechTreeNode());
        node.upChildren.Add(new TechTreeNode());
        node.upChildren.Add(new TechTreeNode());
        node.upChildren[0].tankAddress = "T-34";

        node._child = new TechTreeNode();
        node._child.tankAddress = "T-34";

        node.downChildren.Add(new TechTreeNode());
        node.downChildren.Add(new TechTreeNode());
        node.downChildren[1]._child = new TechTreeNode();
        node.downChildren[1]._child.tankAddress = "T-34";
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

    int row = 0;
    int column = 0;

    private void CreateMode()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup(_countryType);

        var node = _techTree.Root;

        row = 0;
        column = 0;

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        GUILayout.BeginHorizontal();

        while(true)
        {
            node.tankAddress = EditorGUI.TextField(new Rect(10 + column * 100, 110 + row * 100, 100, 20), node.tankAddress);
            ++column;

            if(node._child != null)
            {
                node = node._child;
            }
            else 
            {
                break;
            }
        }

        if(GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(20)))
        {
            var newNode = new TechTreeNode();
            node._child = newNode;
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Create"))
        {
            string path = _techTreeFolderPath + _countryType.ToString() + "TechTree.json";
            string data = JsonConvert.SerializeObject(_techTree, Formatting.None);

            File.WriteAllText(path, data);
        }
    }


    private void EditMode()
    {
        _techTreeFile = (TextAsset)EditorGUILayout.ObjectField(_techTreeFile, typeof(TextAsset), false);

        GUILayout.Label("Developing");
    }
}
