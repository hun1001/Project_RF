using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using System.Text;
using System.Linq;

namespace CustomEditorWindow.TankGenerator
{
    public class TankGenerator : EditorWindow
    {
        private CountryType _countryType = CountryType.None;

        private GameObject[] _tankModels = null;
        private GameObject[] _selectedTankModels = null;
        private Vector2 _modelScrollPos = Vector2.zero;

        private TankSO[] _tankSOs = null;
        private TankSO[] _selectedTankSOs = null;
        private Vector2 _tankSOScrollPos = Vector2.zero;

        private TurretSO[] _turretSOs = null;
        private TurretSO[] _selectedTurretSOs = null;
        private Vector2 _turretSOScrollPos = Vector2.zero;

        private int GeneratedTankCount => _tankModels.Length;

        StringBuilder path = null;

        [MenuItem("Tools/TankGenerator")]
        static void Init()
        {
            TankGenerator window = (TankGenerator)EditorWindow.GetWindow(typeof(TankGenerator));
            float w = 650, h = 310;
            window.position = new Rect(0, 0, w, h);
            window.maxSize = new Vector2(w, h);
            window.minSize = new Vector2(w, h);
            window.Show();
        }

        private static GameObject TankTemplate = null;

        private void OnEnable()
        {
            path = new StringBuilder();

            _tankModels = new GameObject[0];
            _tankSOs = new TankSO[0];
            _turretSOs = new TurretSO[0];

            if (TankTemplate == null)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>("TankTemplate");

                handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
                {
                    TankTemplate = obj.Result;
                };
            }
        }

        private void OnSelectionChange()
        {
            _selectedTankModels = Selection.objects
                .Where(obj => obj is GameObject)
                .Cast<GameObject>()
                .ToArray();

            _selectedTankSOs = Selection.objects
                .Where(obj => obj is TankSO)
                .Cast<TankSO>()
                .ToArray();

            _selectedTurretSOs = Selection.objects
                .Where(obj => obj is TurretSO)
                .Cast<TurretSO>()
                .ToArray();
        }

        private void OnGUI()
        {
            _countryType = (CountryType)EditorGUILayout.EnumPopup("Country Type", _countryType);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            _modelScrollPos = GUILayout.BeginScrollView(_modelScrollPos, GUILayout.Width(215), GUILayout.Height(200));
            foreach (var item in _tankModels)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(item.name);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    _tankModels = _tankModels.Where(x => x != item).ToArray();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Add"))
            {
                _tankModels = _tankModels.Concat(_selectedTankModels).ToArray();
            }
            if (GUILayout.Button("Clear"))
            {
                _tankModels = new GameObject[0];
            }

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            _tankSOScrollPos = GUILayout.BeginScrollView(_tankSOScrollPos, GUILayout.Width(215), GUILayout.Height(200));
            foreach (var item in _tankSOs)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(item.name);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    _tankSOs = _tankSOs.Where(x => x != item).ToArray();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            if (GUILayout.Button("Add"))
            {
                _tankSOs = _tankSOs.Concat(_selectedTankSOs).ToArray();
            }
            if (GUILayout.Button("Clear"))
            {
                _tankSOs = new TankSO[0];
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            _turretSOScrollPos = GUILayout.BeginScrollView(_turretSOScrollPos, GUILayout.Width(215), GUILayout.Height(200));
            foreach (var item in _turretSOs)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(item.name);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    _turretSOs = _turretSOs.Where(x => x != item).ToArray();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            if (GUILayout.Button("Add"))
            {
                _turretSOs = _turretSOs.Concat(_selectedTurretSOs).ToArray();
            }
            if (GUILayout.Button("Clear"))
            {
                _turretSOs = new TurretSO[0];
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUI.enabled = _countryType != CountryType.None && _tankModels != null && _tankSOs != null && _turretSOs != null && TankTemplate != null && _tankModels.Length == _tankSOs.Length && _tankSOs.Length == _turretSOs.Length;
            if (GUILayout.Button("Generate"))
            {
                for (int i = 0; i < GeneratedTankCount; i++)
                {
                    GenerateTank(i);
                }
            }
            GUI.enabled = true;
        }

        private void GenerateTank(int index)
        {
            GameObject tankTemplate = Instantiate(TankTemplate);
            GameObject tankModel = Instantiate(_tankModels[index]);

            tankModel.name = _tankModels[index].name.Replace("(Clone)", "");

            tankTemplate.name = tankModel.name;
            tankModel.transform.SetParent(tankTemplate.transform);

            Tank tank = tankTemplate.GetComponent<Tank>();
            Turret turret = tankTemplate.GetComponent<Turret>();

            tank.ID = tankTemplate.name;
            tank.TankSO = _tankSOs[index];

            turret.TurretSO = _turretSOs[index];
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

            path.Clear();
            DestroyImmediate(tankTemplate);
            DestroyImmediate(tankModel);
        }

        private void OnDisable()
        {
            path = null;
        }
    }
}