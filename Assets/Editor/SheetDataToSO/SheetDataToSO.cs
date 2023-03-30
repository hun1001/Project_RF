using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class SheetDataToSO : EditorWindow
{
    private string _sheetURL = "";
    // https://docs.google.com/spreadsheets/d/1Sph3_eEfKFAfOT_EEN2-XzM9RK7mrly_9FTFSueqgSo/export?format=tsv 탱크

    private string[] _dataType = new string[] { "Tank", "Turret", "Shell" };
    private int _dataTypeIndex = 0;

    private const string _sheetAPI = "export?format=tsv";

    [MenuItem("Window/SheetDataToSO")]
    static void Init()
    {
        SheetDataToSO window = (SheetDataToSO)EditorWindow.GetWindow(typeof(SheetDataToSO));
        window.Show();
    }

    void OnGUI()
    {
        _sheetURL = EditorGUILayout.TextField("Sheet URL", _sheetURL);

        GUILayout.Space(10);

        _dataTypeIndex = EditorGUILayout.Popup("Data Type", selectedIndex: _dataTypeIndex, displayedOptions: _dataType);

        GUILayout.Space(10);

        if (GUILayout.Button("Load"))
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        switch (_dataType[_dataTypeIndex])
        {
            case "Tank":
                UnityWebRequest www = UnityWebRequest.Get(_sheetURL + _sheetAPI);
                www.SendWebRequest();

                while (!www.isDone) { }

                string result = www.downloadHandler.text;
                string result2 = result.Replace("\r", "");

                string[] lines = result2.Split('\n');

                Debug.Log(result);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] datas = lines[i].Split('\t');

                    for (int j = 0; j < datas.Length; j++)
                    {
                        //  Debug.Log(datas[j]);
                    }
                }

                break;
            case "Turret":
                Debug.Log("Turret");
                break;
            case "Shell":
                Debug.Log("Shell");
                break;
            default:
                break;
        }
    }
}
