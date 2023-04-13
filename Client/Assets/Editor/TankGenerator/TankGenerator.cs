using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    private static GameObject TankTemplate = null;

    private void OnEnable()
    {
        if (TankTemplate == null)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("TankTemplate");

            handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
            {
                TankTemplate = obj.Result;
            };
        }
    }

    private void OnGUI()
    {
        _countryType = (CountryType)EditorGUILayout.EnumPopup("Country Type", _countryType);

        _tankModel = (GameObject)EditorGUILayout.ObjectField("Tank Model", _tankModel, typeof(GameObject), false);

        _tankSO = (TankSO)EditorGUILayout.ObjectField("Tank SO", _tankSO, typeof(TankSO), false);

        _turretSO = (TurretSO)EditorGUILayout.ObjectField("Turret SO", _turretSO, typeof(TurretSO), false);

        GUILayout.Space(10);

        GUI.enabled = _countryType != CountryType.None && _tankModel != null && _tankSO != null && _turretSO != null && TankTemplate != null;
        if (GUILayout.Button("Generate"))
        {
            if (_countryType == CountryType.None)
            {
                Debug.LogError("Country Type is None");
            }
            else if (_tankModel == null)
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
        GUI.enabled = true;
    }

    private void GenerateTank()
    {
        // GameObject tankTemplate = Instantiate(TankTemplate);
        // GameObject tankModel;

        // tankTemplate.name = tankModel.name;
        // tankModel.transform.SetParent(tankTemplate.transform);

        // Tank tank = tankTemplate.GetComponent<Tank>();
        // Turret turret = tankTemplate.GetComponent<Turret>();

        // tank.ID = tankTemplate.name;
        // tank.TankSO = _tankSO;

        // turret.TurretSO = _turretSO;
        // turret.TurretTransform = tankModel.transform.GetChild(1);
        // turret.FirePoint = turret.TurretTransform.GetChild(0);

        // if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Tank/" + _countryType.ToString()))
        // {
        //     AssetDatabase.CreateFolder("Assets/Prefabs/Tank", _countryType.ToString());
        // }

        // AssetDatabase.CreateAsset(tankTemplate, "Assets/Prefabs/Tank/" + _countryType.ToString() + "/" + tankTemplate.name + ".prefab");
        // AssetDatabase.SaveAssets();
    }
}
