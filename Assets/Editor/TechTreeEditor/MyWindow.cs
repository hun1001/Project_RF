using UnityEditor;
using UnityEngine;

public class MyWindow : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("c/My Window")]
    public static void ShowWindow()
    {
        GetWindow<MyWindow>("My Window");
    }

    private void OnGUI()
    {
        // ��ũ�Ѻ� ����
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // ��ũ�Ѻ� �ȿ� �� ����
        GUILayout.Label("EditorGUILayout ���");

        // EditorGUI ���
        EditorGUI.LabelField(new Rect(10, 40, position.width - 20, 20), "EditorGUI Labefewqqqqqqqqqqqqqql");

        // ��ũ�Ѻ� ����
        EditorGUILayout.EndScrollView();
    }
}