using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

namespace CustomEditorWindow.SheetDataToSO
{
    public class SheetDataToSO : EditorWindow
    {
        private readonly string[] _dataType = new string[] { "Tank", "Turret", "Shell" };
        private readonly string[] _dataUrlKey = new string[] { "1Sph3_eEfKFAfOT_EEN2-XzM9RK7mrly_9FTFSueqgSo", "1mUDMYbdVgwLQDmMQb2mB6kYl4SzZdbGfOmiaQ9Ww0g4", "16lDqvKtl8077CH5PHDAzojDuWebBP8FXb_VVmwf88J0" };
        private int _dataTypeIndex = 0;

        private const string SheetAPI = "/export?format=tsv";

        [MenuItem("Tools/SheetDataToSO")]
        static void Init()
        {
            SheetDataToSO window = (SheetDataToSO)EditorWindow.GetWindow(typeof(SheetDataToSO));
            int width = 300;
            int height = 80;
            window.position = new Rect(0, 0, width, height);
            window.titleContent = new GUIContent("SheetDataToSO");
            window.maxSize = new Vector2(width, height);
            window.minSize = new Vector2(width, height);
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
            //ResetFolder();

            UnityWebRequest www = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/" + _dataUrlKey[_dataTypeIndex] + SheetAPI);
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

                            asset.SetData(float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[3]), float.Parse(data[4]), float.Parse(data[5]), SheetDataUtil.GetTankType(data[2]), bool.Parse(data[8]), uint.Parse(data[9].ToString()), uint.Parse(data[10].ToString()));

                            if (AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Tank/" + data[1].ToString()) == false)
                            {
                                AssetDatabase.CreateFolder("Assets/ScriptableObjects/Tank", data[1].ToString());
                            }

                            AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Tank/" + data[1].ToString() + "/" + data[0].ToString() + "_TankSO.asset");
                        }
                    }
                    break;
                case "Turret":
                    Debug.Log(result);
                    // for (int i = 1; i < lines.Length; i++)
                    // {
                    //     string[] data = lines[i].Split('\t');

                    //     for (int j = 0; j < data.Length; j++)
                    //     {
                    //         TurretSO asset = ScriptableObject.CreateInstance<TurretSO>();

                    //         asset.ReloadTime = float.Parse(data[2]);
                    //         asset.RotationSpeed = float.Parse(data[3]);
                    //         asset.FOV = float.Parse(data[4]);
                    //         asset.Shells = new List<Shell>();
                    //         asset.Shells.Add(Pool.PoolManager.Load<Shell>("APHE"));
                    //         asset.AtkPower = float.Parse(data[6]);
                    //         asset.PenetrationPower = float.Parse(data[7]);

                    //         if (AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Turret/" + data[1].ToString()) == false)
                    //         {
                    //             AssetDatabase.CreateFolder("Assets/ScriptableObjects/Turret", data[1].ToString());
                    //         }

                    //         AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Turret/" + data[1].ToString() + "/" + data[0].ToString() + "_TurretSO.asset");
                    //     }
                    // }
                    break;
                case "Shell":
                    for (int i = 1; i < lines.Length; ++i)
                    {
                        string[] data = lines[i].Split('\t');

                        for (int j = 0; j < data.Length; ++j)
                        {
                            ShellSO asset = ScriptableObject.CreateInstance<ShellSO>();
                        }
                    }
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


}