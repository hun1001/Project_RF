using System.Linq;
using UnityEngine;
using UnityEditor;

public class ShellGenerator : EditorWindow
{
    private ShellSO[] _shellSOs = null;
    private ShellSO[] _selectedShellSOs = null;
    private Vector2 _shellSOScrollPos = Vector2.zero;

    private Sprite[] _shellSprites = null;
    private Sprite[] _selectedShellSprites = null;
    private Vector2 _shellSpritesScrollPos = Vector2.zero;

    private static GameObject ShellTemplate = null;

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
    }

    private void OnSelectionChange()
    {
        _selectedShellSOs = Selection.objects.OfType<ShellSO>().ToArray();
        _selectedShellSprites = Selection.objects.OfType<Sprite>().ToArray();
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

        }

        GUI.enabled = true;
    }
}
