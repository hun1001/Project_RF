using UnityEngine;
using UnityEditor;
using System.Linq;

public class MyWindow : EditorWindow
{
    private SerializedObject[] selectedObjects;

    [MenuItem("Window/My Window")]
    public static void OpenWindow()
    {
        MyWindow window = GetWindow<MyWindow>();
        window.titleContent = new GUIContent("My Window");
        window.Show();
    }

    private void OnSelectionChange()
    {
        // 선택된 모든 오브젝트를 SerializedObject로 변환합니다.
        selectedObjects = Selection.gameObjects
            .Select(obj => new SerializedObject(obj))
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

        // SerializedProperty를 가져올 프로퍼티 경로를 지정합니다.
        SerializedProperty layerProperty = selectedObjects[0].FindProperty("m_Layer");

        // SerializedProperty를 사용하여 UI를 구성합니다.
        EditorGUILayout.PropertyField(layerProperty);

        EditorGUILayout.Space();

        // 변경된 값을 모든 오브젝트에 적용합니다.
        foreach (var obj in selectedObjects)
        {
            obj.ApplyModifiedProperties();
        }

        EditorGUILayout.EndVertical();
    }
}
