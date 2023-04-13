using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using System.Text;

public class TankGenerator : EditorWindow
{
    private CountryType _countryType = CountryType.None;
    private GameObject _tankModel = null;
    private TankSO _tankSO = null;
    private TurretSO _turretSO = null;

    StringBuilder path = null;

    [MenuItem("Tools/TankGenerator")]
    static void Init()
    {
        TankGenerator window = (TankGenerator)EditorWindow.GetWindow(typeof(TankGenerator));
        window.Show();
    }

    private static GameObject TankTemplate = null;

    private void OnEnable()
    {
        path = new StringBuilder();
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
            GenerateTank();
        }
        GUI.enabled = true;
    }

    private void GenerateTank()
    {
        GameObject tankTemplate = Instantiate(TankTemplate);
        GameObject tankModel = Instantiate(_tankModel);

        tankModel.name = _tankModel.name.Replace("(Clone)", "");

        tankTemplate.name = tankModel.name;
        tankModel.transform.SetParent(tankTemplate.transform);

        Tank tank = tankTemplate.GetComponent<Tank>();
        Turret turret = tankTemplate.GetComponent<Turret>();

        tank.ID = tankTemplate.name;
        tank.TankSO = _tankSO;

        turret.TurretSO = _turretSO;
        turret.TurretTransform = tankModel.transform.GetChild(1);
        turret.FirePoint = turret.TurretTransform.GetChild(0);

        path.Append("Assets/Prefabs/Tank/" + _countryType.ToString());

        if (!AssetDatabase.IsValidFolder(path.ToString()))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs/Tank", _countryType.ToString());
        }

        path.Append("/" + tankTemplate.name + ".prefab");

        if (File.Exists(path.ToString()))
        {
            File.Delete(path.ToString());
        }

        PrefabUtility.SaveAsPrefabAsset(tankTemplate, path.ToString());

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        AddressableAssetGroup group = settings.FindGroup("TankGroup");
        if (group == null)
        {
            group = settings.CreateGroup("TankGroup", false, false, false, null);
        }

        AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path.ToString()), group);

        entry.SetAddress(Path.GetFileNameWithoutExtension(path.ToString()));
        entry.SetAddress(tankTemplate.name);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

        AssetDatabase.SaveAssets();

        DestroyImmediate(tankTemplate);
        DestroyImmediate(tankModel);
    }

    private void OnDisable()
    {
        path.Clear();
        path = null;
    }
}
