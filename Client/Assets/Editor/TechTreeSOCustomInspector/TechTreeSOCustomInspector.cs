using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TechTreeSO))]
public class TechTreeSOCustomInspector : Editor
{
    private TechTreeSO _techTreeSO = null;

    private List<List<Tank>> _tankList = new List<List<Tank>>();
    private TechTreeLinkStateType[][] _isLink = null;

    private uint _lineCount = 0;

    private void OnEnable()
    {
        _techTreeSO = (TechTreeSO)target;

        _tankList.Clear();

        for (int i = 0; i < _techTreeSO.Length; i++)
        {
            _tankList.Add(new List<Tank>());
            for (int j = 0; j < _techTreeSO.GetTankArrayLength(i); j++)
            {
                _tankList[i].Add(_techTreeSO[i, j]);
            }
        }

        if (_tankList.Count > 1)
        {
            _isLink = new TechTreeLinkStateType[_tankList.Count - 1][];
            for (int i = 0; i < _tankList.Count - 1; i++)
            {
                if (i != _tankList.Count - 1)
                {
                    _isLink[i] = new TechTreeLinkStateType[_tankList[i].Count];
                    for (int j = 0; j < _tankList[i].Count; j++)
                    {
                        _isLink[i][j] = _techTreeSO.IsLink(i, j);
                    }
                }
            }
        }

        _lineCount = (uint)_tankList.Count;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Country Settings", EditorStyles.boldLabel);

        _techTreeSO.CountryType = (CountryType)EditorGUILayout.EnumPopup("Country", _techTreeSO.CountryType);
        _techTreeSO.FlagSprite = (Sprite)EditorGUILayout.ObjectField("Flag Sprite", _techTreeSO.FlagSprite, typeof(Sprite), false);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Tech Tree", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        _lineCount = (uint)Mathf.Max(EditorGUILayout.IntField("Line Count", (int)_lineCount), 0);
        if (GUILayout.Button("Add"))
        {
            _lineCount++;
        }
        if (GUILayout.Button("Remove"))
        {
            _lineCount = (uint)Mathf.Max((int)_lineCount - 1, 0);
        }
        EditorGUILayout.EndHorizontal();

        if (_lineCount != _tankList.Count)
        {
            if (_lineCount > _tankList.Count)
            {
                for (int i = _tankList.Count; i < _lineCount; i++)
                {
                    _tankList.Add(new List<Tank>());
                }
            }
            else
            {
                for (int i = _tankList.Count - 1; i >= _lineCount; i--)
                {
                    _tankList.RemoveAt(i);
                }
            }
        }

        EditorGUILayout.Space(5);

        for (int i = 0; i < _tankList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Line " + (i + 1), EditorStyles.boldLabel);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _tankList.RemoveAt(i);
                _lineCount = (uint)Mathf.Max((int)_lineCount - 1, 0);
                i--;
            }
            EditorGUILayout.EndHorizontal();

            for (int j = 0; j < _tankList[i].Count; j++)
            {
                EditorGUILayout.BeginHorizontal();
                _tankList[i][j] = (Tank)EditorGUILayout.ObjectField("Tank " + (j + 1), _tankList[i][j], typeof(Tank), false);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    _tankList[i].RemoveAt(j);
                    j--;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                _tankList[i].Add(null);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (i != _tankList.Count - 1)
            {
                for (int j = 0; j < _isLink[i].Length; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    _isLink[i][j] = (TechTreeLinkStateType)EditorGUILayout.EnumPopup("Link " + (j + 1), _isLink[i][j]);
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
        }

        if (GUI.changed)
        {
            _techTreeSO.SetTankArray(_tankList);
            _techTreeSO.SetIsLink(_isLink);
            EditorUtility.SetDirty(_techTreeSO);
        }
    }

    private void OnDisable()
    {
        _techTreeSO = null;

        _tankList.Clear();
    }
}
