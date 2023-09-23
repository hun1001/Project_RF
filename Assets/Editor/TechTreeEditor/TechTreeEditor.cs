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
            node.tankAddress = i.ToString();
            var newNode = new TechTreeNode();
            node.child = (newNode);
            node = newNode;
        }

        node.tankAddress = "3";

        node.upChild = new TechTreeNode();
        node.upChild.tankAddress = "4";

        node.child = new TechTreeNode();
        node.child.tankAddress = "5";

        node.downChild = new TechTreeNode();
        node.downChild.tankAddress = "6";

        node.downChild.child = new TechTreeNode();
        node.downChild.child.tankAddress = "7";

        node.downChild.child.child = new TechTreeNode();
        node.downChild.child.child.tankAddress = "8";
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

        TechTreeNode node = null;
        Rect rect = new Rect(10, 100, 100, 20);

        TechTreeEditorIterator iterator = new TechTreeEditorIterator(_techTree, rect);

        GUILayout.BeginHorizontal();

        while(iterator.IsSearching)
        {
            node = iterator.GetNextNode();
            rect = iterator.GetNextRect();

            node.tankAddress = EditorGUI.TextField(rect, node.tankAddress);
        }

        GUILayout.EndHorizontal();

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

