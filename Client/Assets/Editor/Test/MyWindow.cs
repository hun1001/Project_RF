using UnityEngine;
using UnityEditor;
using System.Linq;

public class MyWindow : EditorWindow
{
    private GameObject[] selectedObjects;

    [MenuItem("Window/My Window")]
    public static void OpenWindow()
    {
        MyWindow window = GetWindow<MyWindow>();
        window.titleContent = new GUIContent("My Window");
        window.Show();
    }

    private void OnSelectionChange()
    {
        // 선택된 모든 게임오브젝트를 가져옵니다.
        selectedObjects = Selection.objects
            .Where(obj => obj is GameObject)
            .Cast<GameObject>()
            .ToArray();
        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        // 선택된 오브젝트가 없으면 UI를 출력하지 않습니다.
        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorGUILayout.LabelField("No objects selected.");
            EditorGUILayout.EndVertical();
            return;
        }

        // 게임오브젝트를 사용하여 UI를 구성합니다.
        foreach (var obj in selectedObjects)
        {
            EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
        }

        EditorGUILayout.EndVertical();
    }
}
