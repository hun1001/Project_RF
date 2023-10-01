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

    private readonly string _techTreeFolderPath = Application.dataPath + "/TechTreeInformation/";

    private TextAsset _techTreeInformationFile = null;
    private TechTreeInformation _techTreeInformation = null;

    private TechTreeEditorMode _mode = TechTreeEditorMode.None;
    private CountryType _countryType = CountryType.None;

    private Rect _scrollViewRect = new Rect();
    private Vector2 _scrollPosition = Vector2.zero;
    private Rect _contentRect = new Rect();


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
        _techTreeInformationFile = null;
        _techTreeInformation = null;
    }

    private void OnCreateModeChanged()
    {
        _techTreeInformationFile = null;
        _techTreeInformation = new TechTreeInformation();
    }

    private void OnEditModeChanged()
    {
        _techTreeInformation = null;
        _techTreeInformationFile = null;
    }

    private void NoneMode()
    {
        GUILayout.Label("Please Select Mode");
    }


    private void CreateMode()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup(_countryType);
        _techTreeInformation.Country = _countryType;

        int techTreeCount = EditorGUILayout.IntField("TechTree Count", _techTreeInformation.techTreeList.Count);

        SetTechTreeListCount(techTreeCount);

        _scrollViewRect = new Rect(0, 40, position.width, position.height - 10);

        _scrollPosition = GUI.BeginScrollView(_scrollViewRect, _scrollPosition, _contentRect);

        ShowTechTreeInformation();

        GUI.EndScrollView();

        GUI.enabled = _countryType != CountryType.None && TankAddressInspection(_techTreeInformation);

        if (GUI.Button(new Rect(0, position.height - 30, position.width, 30), "Create"))
        {
            string path = _techTreeFolderPath + _countryType.ToString() + "TechTree.json";
            string data = JsonConvert.SerializeObject(_techTreeInformation, Formatting.None);

            File.WriteAllText(path, data);

            var file = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            //AddressablesManager.Instance.CreateAddressableAsset(path, _countryType.ToString() + "TechTree", "TechTreeGroup", "TechTree");

            _techTreeInformation = new TechTreeInformation();
        }

        GUI.enabled = true;
    }

    private void EditMode()
    {
        TextAsset beforeTextAsset = _techTreeInformationFile;
        _techTreeInformationFile = (TextAsset)EditorGUILayout.ObjectField(_techTreeInformationFile, typeof(TextAsset), false);

        if(beforeTextAsset != _techTreeInformationFile)
        {
            _techTreeInformation = JsonConvert.DeserializeObject<TechTreeInformation>(_techTreeInformationFile.text);
            _countryType = _techTreeInformation.Country;
        }

        if (_techTreeInformation != null)
        {
            int techTreeCount = EditorGUILayout.IntField("TechTree Count", _techTreeInformation.techTreeList.Count);

            SetTechTreeListCount(techTreeCount);

            _scrollViewRect = new Rect(0, 40, position.width, position.height - 10);

            _scrollPosition = GUI.BeginScrollView(_scrollViewRect, _scrollPosition, _contentRect);

            ShowTechTreeInformation();

            GUI.EndScrollView();

            GUI.enabled = TankAddressInspection(_techTreeInformation);

            if (GUILayout.Button("Modify"))
            {
                _techTreeInformationFile = null;

                string path = _techTreeFolderPath + _countryType.ToString() + "TechTree.json";
                string data = JsonConvert.SerializeObject(_techTreeInformation, Formatting.None);

                File.WriteAllText(path, data);

                _techTreeInformation = null;
            }

            GUI.enabled = true;
        }
    }

    private void ShowTechTreeInformation()
    {
        TechTree techTree = null;
        TechTreeNode node = null;
        Rect rect = new Rect(10, 100, 100, 20);

        for (int i = 0; i < _techTreeInformation.techTreeList.Count; ++i)
        {
            techTree = _techTreeInformation.techTreeList[i];
            TechTreeEditorBFSIterator iterator = new TechTreeEditorBFSIterator(techTree, rect);

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

            rect = new Rect(10, iterator.MaxY + (techTree.GetWidth() * 30) + 100, 100, 20);

            _contentRect = new Rect(0, 0, Mathf.Max(iterator.MaxX, _contentRect.width) + 20, Mathf.Max(iterator.MaxY, _contentRect.height) + 20);
        }
    }

    private void SetTechTreeListCount(int count)
    {
        if (_techTreeInformation.techTreeList.Count < count)
        {
            for (int i = _techTreeInformation.techTreeList.Count; i < count; i++)
            {
                _techTreeInformation.techTreeList.Add(new TechTree());
            }
        }
        else if (_techTreeInformation.techTreeList.Count > count)
        {
            for (int i = _techTreeInformation.techTreeList.Count - 1; i >= count; i--)
            {
                _techTreeInformation.techTreeList.RemoveAt(i);
            }
        }
    }

    private bool TankAddressInspection(TechTreeInformation techTreeInformation)
    {
        for (int i = 0; i < techTreeInformation.techTreeList.Count; i++)
        {
            TechTreeBFSIterator iterator = new TechTreeBFSIterator(techTreeInformation.techTreeList[i]);
            var tanks = AddressablesManager.Instance.GetLabelResourcesComponents<Tank>("Tank");

            while (iterator.IsSearching)
            {
                TechTreeNode node = iterator.GetNextNode();

                if (tanks.ToList().Find(tank => tank.ID == node.tankAddress) == null)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}
