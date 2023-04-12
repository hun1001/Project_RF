using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TankGenerator : EditorWindow
{
    private CountryType _countryType = CountryType.None;
    private GameObject _tankModel = null;

    [MenuItem("Tools/Tank Generator")]
    static void Init()
    {
        TankGenerator window = (TankGenerator)EditorWindow.GetWindow(typeof(TankGenerator));
        window.Show();
    }

    private void OnGUI()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup("Country Type", _countryType);

        _tankModel = (GameObject)EditorGUILayout.ObjectField("Tank Model", _tankModel, typeof(GameObject), false);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate"))
        {
            GenerateTank();
        }
    }

    private void GenerateTank()
    {

        Debug.Log("Generate Tank");
    }
}
