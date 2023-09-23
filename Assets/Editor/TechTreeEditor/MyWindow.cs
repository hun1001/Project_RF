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
        // 스크롤뷰 시작
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // 스크롤뷰 안에 들어갈 내용
        GUILayout.Label("EditorGUILayout 요소");

        // EditorGUI 요소
        EditorGUI.LabelField(new Rect(10, 40, position.width - 20, 20), "EditorGUI Labefewqqqqqqqqqqqqqql");

        // 스크롤뷰 종료
        EditorGUILayout.EndScrollView();
    }
}