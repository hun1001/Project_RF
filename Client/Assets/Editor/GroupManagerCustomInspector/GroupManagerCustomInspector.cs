using System.Collections;
using System.Collections.Generic;
using FoW;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Pool;
using System;

namespace CustomEditorInspector.Group
{
    [CustomEditor(typeof(GroupManager))]
    public class GroupManagerCustomInspector : Editor
    {
        private GroupManager _groupManager = null;
        private MapSO _mapData = null;

        private List<FogOfWarTeam> _groupList = null;
        private List<Color> _groupColorList = null;
        private uint _childCount = 0;

        private int _groupCount = 0;

        private void OnEnable()
        {
            _groupManager = (GroupManager)target;

            _groupList = new List<FogOfWarTeam>();
            _groupColorList = new List<Color>();
            _mapData = MapManager.Instance.MapData;

            for (int i = 0; i < Enum.GetValues(typeof(GroupType)).Length - 1; i++)
            {
                _groupColorList.Add(Color.white);
            }

            UpdateGroups();
        }

        public override void OnInspectorGUI()
        {
            if (_childCount != _groupManager.transform.childCount)
            {
                UpdateGroups();
            }

            GUILayout.BeginVertical();

            for (int i = 0; i < _childCount; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(_groupList[i].name);
                _groupColorList[i] = EditorGUILayout.ColorField(_groupColorList[i], GUILayout.Width(200));
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            GUI.enabled = Enum.GetValues(typeof(GroupType)).Length - 1 > _groupCount;

            if (GUILayout.Button("Add"))
            {
                var teamGroup = PoolManager.Get<FogOfWarTeam>("TeamGroup", _groupManager.transform);
                teamGroup.team = _groupCount;
                teamGroup.name = $"TeamGroup_{Enum.GetName(typeof(GroupType), (GroupType)(_groupCount))}";
                teamGroup.mapSize = _mapData.MapSize;
                teamGroup.mapResolution = _mapData.MapResolution;
                teamGroup.mapOffset = _mapData.MapOffset;

                UpdateGroups();
                ++_groupCount;
            }

            GUI.enabled = true;

            GUI.enabled = _childCount > 0;

            if (GUILayout.Button("Remove"))
            {
                if (_childCount > 0)
                {
                    DestroyImmediate(_groupList[(int)_childCount - 1].gameObject);
                }
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                _groupManager.GroupColorList = _groupColorList;
            }
        }

        private void UpdateGroups()
        {
            _groupList.Clear();
            _groupList = _groupManager.GetComponentsInChildren<FogOfWarTeam>().ToList();

            if (_groupManager.GroupColorList.Count == Enum.GetValues(typeof(GroupType)).Length - 1)
            {
                _groupColorList.Clear();
                _groupColorList = _groupManager.GroupColorList;
            }

            _childCount = (uint)_groupList.Count;
            _groupCount = _groupList.Count;
        }
    }
}