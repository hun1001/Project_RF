using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class ShellGenerator : EditorWindow
{
    private ShellSO[] _shellSOs = null;
    private ShellSO[] _selectedShellSOs = null;
    private Vector2 _shellSOScrollPos = Vector2.zero;

    private Sprite[] _shellSprites = null;
    private Sprite[] _selectedShellSprites = null;
    private Vector2 _shellSpritesScrollPos = Vector2.zero;

    private static GameObject ShellTemplate = null;

    private StringBuilder _path = null;

    [MenuItem("Tools/ShellGenerator")]
    static void Init()
    {
        ShellGenerator window = (ShellGenerator)EditorWindow.GetWindow(typeof(ShellGenerator));
        window.Show();
    }

    private void OnEnable()
    {
        _shellSOs = new ShellSO[0];
        _shellSprites = new Sprite[0];
        _path = new StringBuilder();

        if (ShellTemplate == null)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("ShellTemplate");

            handle.Completed += (AsyncOperationHandle<GameObject> obj) =>
            {
                ShellTemplate = obj.Result;
            };
        }
    }

    private void OnSelectionChange()
    {
        _selectedShellSOs = Selection.objects.OfType<ShellSO>().ToArray();
        _selectedShellSprites = Selection.objects.OfType<Texture2D>().Select(x => AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(x))).ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Shell SO");
        GUILayout.FlexibleSpace();
        GUILayout.Label(_shellSOs.Length.ToString());
        GUILayout.EndHorizontal();
        _shellSOScrollPos = GUILayout.BeginScrollView(_shellSOScrollPos, GUILayout.Width(215), GUILayout.Height(200));
        foreach (var item in _shellSOs)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(item.name);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _shellSOs = _shellSOs.Where(x => x != item).ToArray();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Add"))
        {
            _shellSOs = _shellSOs.Concat(_selectedShellSOs).ToArray();
        }
        if (GUILayout.Button("Clear"))
        {
            _shellSOs = new ShellSO[0];
        }

        GUILayout.EndVertical();

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Shell Sprite");
        GUILayout.FlexibleSpace();
        GUILayout.Label(_shellSprites.Length.ToString());
        GUILayout.EndHorizontal();

        _shellSOScrollPos = GUILayout.BeginScrollView(_shellSOScrollPos, GUILayout.Width(215), GUILayout.Height(200));
        foreach (var item in _shellSprites)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(item.name);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _shellSprites = _shellSprites.Where(x => x != item).ToArray();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        if (GUILayout.Button("Add"))
        {
            _shellSprites = _shellSprites.Concat(_selectedShellSprites).ToArray();
        }
        if (GUILayout.Button("Clear"))
        {
            _shellSprites = new Sprite[0];
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUI.enabled = _shellSOs != null && _shellSprites != null && ShellTemplate != null && _shellSOs.Length == _shellSprites.Length;
        if (GUILayout.Button("Generate"))
        {
            for (int i = 0; i < _shellSOs.Length; i++)
            {
                GenerateShell(i);
            }
            ResetAllSettingData();
            AssetDatabase.SaveAssets();
        }

        GUI.enabled = true;
    }

    private void GenerateShell(int index)
    {
        bool isExist = File.Exists("Assets/Prefabs/Shell/" + _shellSOs[index].name.Replace("_ShellSO", "") + ".prefab");
        GameObject shellTemplate = null;

        if (isExist)
        {
            shellTemplate = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Shell/" + _shellSOs[index].name.Replace("_ShellSO", "") + ".prefab");

            var shell = shellTemplate.GetComponent<Shell>();
            shell.SetShellPrefabs(_shellSOs[index].Code, _shellSOs[index], _shellSprites[index]);
        }
        else
        {
            shellTemplate = Instantiate(ShellTemplate, Vector3.zero, Quaternion.identity);

            shellTemplate.name = _shellSOs[index].name.Replace("_ShellSO", "");

            var shell = shellTemplate.GetComponent<Shell>();
            shell.SetShellPrefabs(_shellSOs[index].Code, _shellSOs[index], _shellSprites[index]);

            _path.Clear();
            _path.Append("Assets/Prefabs/Shell/" + shellTemplate.name + ".prefab");

            if (File.Exists(_path.ToString()))
            {
                File.Delete(_path.ToString());
            }

            PrefabUtility.SaveAsPrefabAsset(shellTemplate, _path.ToString());

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            var group = settings.FindGroup("ShellGroup");

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(_path.ToString()), group);

            entry.address = shell.ShellSO.Code;
            entry.SetLabel("Shell", true);

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

            DestroyImmediate(shellTemplate);
        }
    }

    private void ResetAllSettingData()
    {
        _shellSOs = new ShellSO[0];
        _shellSprites = new Sprite[0];
    }
}
