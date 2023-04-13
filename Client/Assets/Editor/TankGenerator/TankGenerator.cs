using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TankGenerator : EditorWindow
{
    private CountryType _countryType = CountryType.None;
    private GameObject _tankModel = null;
    private TankSO _tankSO = null;
    private TurretSO _turretSO = null;

    [MenuItem("Tools/TankGenerator")]
    static void Init()
    {
        TankGenerator window = (TankGenerator)EditorWindow.GetWindow(typeof(TankGenerator));
        window.Show();
    }

    private void OnGUI()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup("Country Type", _countryType);

        _tankModel = (GameObject)EditorGUILayout.ObjectField("Tank Model", _tankModel, typeof(GameObject), false);

        _tankSO = (TankSO)EditorGUILayout.ObjectField("Tank SO", _tankSO, typeof(TankSO), false);

        _turretSO = (TurretSO)EditorGUILayout.ObjectField("Turret SO", _turretSO, typeof(TurretSO), false);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate"))
        {
            if (_tankModel == null)
            {
                Debug.LogError("Tank Model is null");
            }
            else if (_tankSO == null)
            {
                Debug.LogError("TankSO is null");
            }
            else if (_turretSO == null)
            {
                Debug.LogError("TurretSO is null");
            }
            else
            {
                GenerateTank();
            }
        }
    }

    private void GenerateTank()
    {
        GameObject tankTemplate = Addressable.AddressablesManager.Instance.GetResource<GameObject>("TankTemplate");
        GameObject tankModel = Instantiate(_tankModel);

        while (tankTemplate == null)
        {

        }

        Debug.Log(tankTemplate);
    }
}
