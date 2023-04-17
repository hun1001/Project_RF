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
        private uint _childCount = 0;

        private int _groupCount = 0;

        private void OnEnable()
        {
            _groupManager = (GroupManager)target;

            _groupList = new List<FogOfWarTeam>();
            _mapData = MapManager.Instance.MapData;

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
                GUILayout.Label(_groupList[i].name);
            }

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            GUI.enabled = Enum.GetValues(typeof(GroupType)).Length - 1 > _groupCount;

            if (GUILayout.Button("Add"))
            {
                var teamGroup = PoolManager.Get<FogOfWarTeam>("TeamGroup", _groupManager.transform);
                teamGroup.team = ++_groupCount;
                teamGroup.name = $"TeamGroup_{Enum.GetName(typeof(GroupType), (GroupType)(_groupCount))}";
                teamGroup.mapSize = _mapData.MapSize;
                teamGroup.mapResolution = _mapData.MapResolution;
                teamGroup.mapOffset = _mapData.MapOffset;
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
        }

        private void UpdateGroups()
        {
            _groupList.Clear();
            _groupList = _groupManager.GetComponentsInChildren<FogOfWarTeam>().ToList();
            _childCount = (uint)_groupList.Count;
            _groupCount = _groupList.Count;
        }
    }
}