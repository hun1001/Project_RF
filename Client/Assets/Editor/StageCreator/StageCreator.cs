using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Stage;

public class StageCreator : EditorWindow
{
    [SerializeField]
    public Tank[] ins_Tanks;

    [MenuItem("Tools/StageCreator")]
    static void Init()
    {
        StageCreator window = GetWindow<StageCreator>();
        window.titleContent = new GUIContent("StageCreator");
        window.Show();
    }

    private Editor _editor;
    private string _fileName = null;
    private int _rewardValue = 0;
    private int _minDefeatPercent = 0;
    private int _maxDefeatPercent = 0;

    private void OnGUI()
    {
        if (!_editor) { _editor = Editor.CreateEditor(this); }
        if (_editor) { _editor.OnInspectorGUI(); }

        GUILayout.BeginVertical("BOX");

        GUILayout.Space(10);
        GUILayout.Label("Stage Name", GUILayout.Width(75));
        _fileName = EditorGUILayout.TextField(_fileName, GUILayout.ExpandWidth(true));
        GUILayout.Label("Reward", GUILayout.ExpandWidth(true));
        _rewardValue = EditorGUILayout.IntField(_rewardValue, GUILayout.ExpandWidth(true));
        GUILayout.Label("Min Defeat Percent (0% ~ 50%)", GUILayout.ExpandWidth(true));
        _minDefeatPercent = EditorGUILayout.IntField(_minDefeatPercent, GUILayout.ExpandWidth(true));
        GUILayout.Label("Max Defeat Percent (0% ~ 50%)", GUILayout.ExpandWidth(true));
        _maxDefeatPercent = EditorGUILayout.IntField(_maxDefeatPercent, GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create"))
        {
            if (ins_Tanks.Length == 0)
            {
                Debug.LogError("Tank List is Empty!");
            }
            else if (_fileName == "" || _fileName == null)
            {
                Debug.LogError("File name is Null!");
            }
            else if (_rewardValue < 0)
            {
                Debug.LogError("Reward value is negative!");
            }
            else if (_minDefeatPercent < 0 || _minDefeatPercent > 50)
            {
                Debug.LogError("Min Percent is out of realm");
            }
            else if (_maxDefeatPercent < 0 || _maxDefeatPercent > 50)
            {
                Debug.LogError("Max Percent is out of realm");
            }
            else
            {
                StageSO stageSO = ScriptableObject.CreateInstance<StageSO>();

                stageSO.SetData(ins_Tanks, _rewardValue, _minDefeatPercent, _maxDefeatPercent);
                AssetDatabase.CreateAsset(stageSO, "Assets/ScriptableObjects/GameWay/Stage/" + _fileName + ".asset");
                EditorUtility.SetDirty(stageSO);
            }
        }
    }
}

[CustomEditor(typeof(StageCreator), true)]
public class StageCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var list = serializedObject.FindProperty("ins_Tanks");
        EditorGUILayout.PropertyField(list, new GUIContent("Tank List"), true);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("List Reset"))
        {
            list.arraySize = 0;
        }
    }
}
