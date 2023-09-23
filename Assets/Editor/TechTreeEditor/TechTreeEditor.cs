using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Addressable;
using Unity.Plastic.Newtonsoft.Json;
using System.Linq;

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
        _techTree.Country = _countryType;

        TechTreeNode node = null;
        Rect rect = new Rect(10, 100, 100, 20);

        TechTreeEditorIterator iterator = new TechTreeEditorIterator(_techTree, rect);

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        while(iterator.IsSearching)
        {
            node = iterator.GetNextNode();
            rect = iterator.GetNextRect();

            node.tankAddress = EditorGUI.TextField(rect, node.tankAddress);

            bool beforeHasUpChild = node.hasUpChild;
            bool beforeHasChild = node.hasChild;
            bool beforeHasDownChild = node.hasDownChild;

            if(node.hasUpChild)
            {
                Handles.DrawLine(new Vector3(rect.x + 50, rect.y, 0), new Vector3(rect.x + 50, rect.y - 50, 0));
                Handles.DrawLine(new Vector3(rect.x + 50, rect.y - 50, 0), new Vector3(rect.x + 140, rect.y - 50, 0));
            }

            if(node.hasChild)
            {
                Handles.DrawLine(new Vector3(rect.x + 100, rect.y + 10, 0), new Vector3(rect.x + 140, rect.y + 10, 0));
            }

            if(node.hasDownChild)
            {
                Handles.DrawLine(new Vector3(rect.x + 50, rect.y + 20, 0), new Vector3(rect.x + 50, rect.y + 70, 0));
                Handles.DrawLine(new Vector3(rect.x + 50, rect.y + 70, 0), new Vector3(rect.x + 140, rect.y + 70, 0));
            }

            node.hasUpChild = GUI.Toggle(new Rect(rect.x + 105, rect.y - 20, 20, 20), node.hasUpChild, "");
            node.hasChild = GUI.Toggle(new Rect(rect.x + 105, rect.y, 20, 20), node.hasChild, "");
            node.hasDownChild = GUI.Toggle(new Rect(rect.x + 105, rect.y + 20, 20, 20), node.hasDownChild, "");

            if (beforeHasUpChild != node.hasUpChild)
            {
                if (node.hasUpChild)
                {
                    node.upChild = new TechTreeNode();
                }
                else
                {
                    node.upChild = null;
                }
            }

            if(beforeHasChild != node.hasChild)
            {
                if (node.hasChild)
                {
                    node.child = new TechTreeNode();
                }
                else
                {
                    node.child = null;
                }
            }

            if(beforeHasDownChild != node.hasDownChild)
            {
                if (node.hasDownChild)
                {
                    node.downChild = new TechTreeNode();
                }
                else
                {
                    node.downChild = null;
                }
            }
        }

        GUILayout.EndScrollView();

        GUI.enabled = _countryType != CountryType.None && TankAddressInspection(_techTree);

        if (GUILayout.Button("Create"))
        {
            string path = _techTreeFolderPath + _countryType.ToString() + "TechTree.json";
            string data = JsonConvert.SerializeObject(_techTree, Formatting.None);

            File.WriteAllText(path, data);

            _techTree = new TechTree();
        }

        GUI.enabled = true;
    }

    private void EditMode()
    {
        TextAsset beforeTextAsset = _techTreeFile;
        _techTreeFile = (TextAsset)EditorGUILayout.ObjectField(_techTreeFile, typeof(TextAsset), false);

        if(beforeTextAsset != _techTreeFile)
        {
            _techTree = JsonConvert.DeserializeObject<TechTree>(_techTreeFile.text);
            _countryType = _techTree.Country;
        }

        if (_techTree != null)
        {
            TechTreeNode node = null;
            Rect rect = new Rect(10, 100, 100, 20);

            TechTreeEditorIterator iterator = new TechTreeEditorIterator(_techTree, rect);

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            while (iterator.IsSearching)
            {
                node = iterator.GetNextNode();
                rect = iterator.GetNextRect();

                node.tankAddress = EditorGUI.TextField(rect, node.tankAddress);

                bool beforeHasUpChild = node.hasUpChild;
                bool beforeHasChild = node.hasChild;
                bool beforeHasDownChild = node.hasDownChild;

                if (node.hasUpChild)
                {
                    Handles.DrawLine(new Vector3(rect.x + 50, rect.y, 0), new Vector3(rect.x + 50, rect.y - 50, 0));
                    Handles.DrawLine(new Vector3(rect.x + 50, rect.y - 50, 0), new Vector3(rect.x + 140, rect.y - 50, 0));
                }

                if (node.hasChild)
                {
                    Handles.DrawLine(new Vector3(rect.x + 100, rect.y + 10, 0), new Vector3(rect.x + 140, rect.y + 10, 0));
                }

                if (node.hasDownChild)
                {
                    Handles.DrawLine(new Vector3(rect.x + 50, rect.y + 20, 0), new Vector3(rect.x + 50, rect.y + 70, 0));
                    Handles.DrawLine(new Vector3(rect.x + 50, rect.y + 70, 0), new Vector3(rect.x + 140, rect.y + 70, 0));
                }

                node.hasUpChild = GUI.Toggle(new Rect(rect.x + 105, rect.y - 20, 20, 20), node.hasUpChild, "");
                node.hasChild = GUI.Toggle(new Rect(rect.x + 105, rect.y, 20, 20), node.hasChild, "");
                node.hasDownChild = GUI.Toggle(new Rect(rect.x + 105, rect.y + 20, 20, 20), node.hasDownChild, "");

                if (beforeHasUpChild != node.hasUpChild)
                {
                    if (node.hasUpChild)
                    {
                        node.upChild = new TechTreeNode();
                    }
                    else
                    {
                        node.upChild = null;
                    }
                }

                if (beforeHasChild != node.hasChild)
                {
                    if (node.hasChild)
                    {
                        node.child = new TechTreeNode();
                    }
                    else
                    {
                        node.child = null;
                    }
                }

                if (beforeHasDownChild != node.hasDownChild)
                {
                    if (node.hasDownChild)
                    {
                        node.downChild = new TechTreeNode();
                    }
                    else
                    {
                        node.downChild = null;
                    }
                }
            }

            GUILayout.EndScrollView();

            GUI.enabled = TankAddressInspection(_techTree);

            if (GUILayout.Button("Modify"))
            {
                _techTreeFile = null;

                string path = _techTreeFolderPath + _countryType.ToString() + "TechTree.json";
                string data = JsonConvert.SerializeObject(_techTree, Formatting.None);

                File.WriteAllText(path, data);

                _techTree = null;
            }

            GUI.enabled = true;
        }
    }

    private bool TankAddressInspection(TechTree techTree)
    {
        TechTreeIterator iterator = new TechTreeIterator(techTree);
        var tanks = AddressablesManager.Instance.GetLabelResourcesComponents<Tank>("Tank");

        while(iterator.IsSearching)
        {
            TechTreeNode node = iterator.GetNextNode();

            if(tanks.ToList().Find(tank => tank.ID == node.tankAddress) == null)
            {
                return false;
            }
        }
        
        return true;
    }
}
