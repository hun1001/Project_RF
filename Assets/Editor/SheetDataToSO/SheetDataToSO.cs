using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class SheetDataToSO : EditorWindow
{
    private string[] _dataType = new string[] { "Tank", "Turret", "Shell" };
    private string[] _dataUrlKey = new string[] { "1Sph3_eEfKFAfOT_EEN2-XzM9RK7mrly_9FTFSueqgSo", "1mUDMYbdVgwLQDmMQb2mB6kYl4SzZdbGfOmiaQ9Ww0g4", "16lDqvKtl8077CH5PHDAzojDuWebBP8FXb_VVmwf88J0" };
    private int _dataTypeIndex = 0;

    private const string _sheetAPI = "/export?format=tsv";

    [MenuItem("Window/SheetDataToSO")]
    static void Init()
    {
        SheetDataToSO window = (SheetDataToSO)EditorWindow.GetWindow(typeof(SheetDataToSO));
        window.Show();
    }

    void OnGUI()
    {
        _dataTypeIndex = EditorGUILayout.Popup("Data Type", selectedIndex: _dataTypeIndex, displayedOptions: _dataType);

        GUILayout.Space(10);

        if (GUILayout.Button("Load"))
        {
            LoadData();
        }

        if (GUILayout.Button("Reset"))
        {
            ResetFolder();
        }
    }

    private void LoadData()
    {
        ResetFolder();

        UnityWebRequest www = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/" + _dataUrlKey[_dataTypeIndex] + _sheetAPI);
        www.SendWebRequest();

        while (!www.isDone) { }

        string result = www.downloadHandler.text;
        string result2 = result.Replace("\r", "");

        string[] lines = result2.Split('\n');

        switch (_dataType[_dataTypeIndex])
        {
            case "Tank":
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split('\t');

                    for (int j = 0; j < data.Length; j++)
                    {
                        TankSO asset = ScriptableObject.CreateInstance<TankSO>();

                        asset.Acceleration = float.Parse(data[1]);
                        data[2] = data[2].Replace("km/h", "");
                        asset.MaxSpeed = float.Parse(data[2]);
                        asset.RotationSpeed = float.Parse(data[3]);
                        asset.Armour = float.Parse(data[4]);
                        asset.HP = float.Parse(data[5]);
                        asset.TankType = TankType.Medium;

                        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Tank/" + data[0].ToString() + "_TankSO.asset");
                    }
                }
                break;
            case "Turret":
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split('\t');

                    for (int j = 0; j < data.Length; j++)
                    {
                        TurretSO asset = ScriptableObject.CreateInstance<TurretSO>();

                        asset.Power = float.Parse(data[1]);
                        asset.ReloadTime = float.Parse(data[2]);
                        data[3] = data[3].Replace("deg/s", "");
                        asset.RotationSpeed = float.Parse(data[3]);

                        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Turret/" + data[0].ToString() + "_TurretSO.asset");
                    }
                }
                break;
            case "Shell":
                Debug.Log("Shell is not implemented yet.");
                break;
            default:
                break;
        }
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
    }

    private void ResetFolder()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/ScriptableObjects/" + _dataType[_dataTypeIndex]);

        foreach (var item in dir.GetFiles())
        {
            item.Delete();
        }

        foreach (DirectoryInfo di in dir.GetDirectories())
        {
            di.Delete(true);
        }

        AssetDatabase.Refresh();
    }
}
