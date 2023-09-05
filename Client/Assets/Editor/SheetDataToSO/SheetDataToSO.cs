using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

namespace CustomEditorWindow.SheetDataToSO
{
    public class SheetDataToSO : EditorWindow
    {
        private readonly string[] _dataType = new string[] { "Tank", "Turret", "Shell", "Tip" };
        private readonly string[] _dataUrlKey = new string[] { "1Sph3_eEfKFAfOT_EEN2-XzM9RK7mrly_9FTFSueqgSo", "1mUDMYbdVgwLQDmMQb2mB6kYl4SzZdbGfOmiaQ9Ww0g4", "16lDqvKtl8077CH5PHDAzojDuWebBP8FXb_VVmwf88J0", "1q9cJgNMmeD26cN2W1D78cG-LoPnsjSMkxWXqPQ7Ggkk" };
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
            UnityWebRequest www = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/" + _dataUrlKey[_dataTypeIndex] + SheetAPI);
            www.SendWebRequest();

            while (!www.isDone) { }

            string result = www.downloadHandler.text;
            string result2 = result.Replace("\r", "");

            string[] lines = result2.Split('\n');

            bool isExist;

            try
            {
                switch (_dataType[_dataTypeIndex])
                {
                    case "Tank":

                        // 0: Name
                        // 1: Tier
                        // 2: Country
                        // 3: Category
                        // 4: MaxSpeed
                        // 5: Acceleration
                        // 6: RotationSpeed
                        // 7: HP
                        // 8: Armour
                        // 9: Price

                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] data = lines[i].Split('\t');

                            TankSO asset = null;

                            isExist = File.Exists("Assets/ScriptableObjects/Tank/" + data[2].ToString() + "/" + data[0].ToString() + "_TankSO.asset");

                            if (isExist)
                            {
                                asset = AssetDatabase.LoadAssetAtPath<TankSO>("Assets/ScriptableObjects/Tank/" + data[2].ToString() + "/" + data[0].ToString() + "_TankSO.asset");
                            }
                            else
                            {
                                asset = ScriptableObject.CreateInstance<TankSO>();
                            }

                            asset.SetData(float.Parse(data[7]), float.Parse(data[8]), float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]), SheetDataUtil.GetCountryType(data[2]), SheetDataUtil.GetTankType(data[3]), uint.Parse(data[1]), uint.Parse(data[9]));

                            if (AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Tank/" + data[2].ToString()) == false)
                            {
                                AssetDatabase.CreateFolder("Assets/ScriptableObjects/Tank", data[2].ToString());
                            }

                            if (isExist == false)
                            {
                                AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Tank/" + data[2].ToString() + "/" + data[0].ToString() + "_TankSO.asset");
                            }
                            EditorUtility.SetDirty(asset);
                        }
                        break;
                    case "Turret":
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] data = lines[i].Split('\t');

                            TurretSO asset = null;
                            isExist = File.Exists("Assets/ScriptableObjects/Turret/" + data[1].ToString() + "/" + data[0].ToString() + "_TurretSO.asset");

                            if (isExist)
                            {
                                asset = AssetDatabase.LoadAssetAtPath<TurretSO>("Assets/ScriptableObjects/Turret/" + data[1].ToString() + "/" + data[0].ToString() + "_TurretSO.asset");
                            }
                            else
                            {
                                asset = ScriptableObject.CreateInstance<TurretSO>();
                            }

                            if (data[2].Contains(";"))
                            {
                                asset.IsBurst = true;
                                string[] datas = data[2].Split(';');
                                asset.BurstData.BurstReloadTime = float.Parse(datas[0]);
                                asset.BurstData.MagazineSize = int.Parse(datas[1]);
                                asset.ReloadTime = float.Parse(datas[2]);
                            }
                            else
                            {
                                asset.ReloadTime = float.Parse(data[2]);
                            }
                            asset.RotationSpeed = float.Parse(data[3]);
                            asset.FOV = float.Parse(data[4]);
                            asset.Shells = SheetDataUtil.GetUseShell(data[5]);
                            asset.AtkPower = float.Parse(data[6]);
                            asset.PenetrationPower = float.Parse(data[7]);

                            if (AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Turret/" + data[1].ToString()) == false)
                            {
                                AssetDatabase.CreateFolder("Assets/ScriptableObjects/Turret", data[1].ToString());
                            }

                            if (isExist == false)
                            {
                                AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Turret/" + data[1].ToString() + "/" + data[0].ToString() + "_TurretSO.asset");
                            }
                            EditorUtility.SetDirty(asset);
                        }
                        break;
                    case "Shell":
                        for (int i = 1; i < lines.Length; ++i)
                        {
                            string[] data = lines[i].Split('\t');

                            ShellSO asset;
                            isExist = File.Exists("Assets/ScriptableObjects/Shell/" + data[0].ToString() + "_ShellSO.asset");

                            if (isExist)
                            {
                                asset = AssetDatabase.LoadAssetAtPath<ShellSO>("Assets/ScriptableObjects/Shell/" + data[0].ToString() + "_ShellSO.asset");
                            }
                            else
                            {
                                asset = ScriptableObject.CreateInstance<ShellSO>();
                            }

                            asset.Code = data[1];
                            asset.Damage = float.Parse(data[2]);
                            asset.Penetration = float.Parse(data[3]);
                            asset.Speed = float.Parse(data[4]);
                            asset.RicochetAngle = float.Parse(data[5]);

                            if (isExist == false)
                            {
                                AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Shell/" + data[0].ToString() + "_ShellSO.asset");
                            }
                            EditorUtility.SetDirty(asset);
                        }
                        break;
                    case "Tip":
                        for(int i = 1;i<lines.Length;++i)
                        {
                            string[] data = lines[i].Split('\t');

                            string path = "Assets/ScriptableObjects/Tip/TipSO" + data[1].ToString() + ".asset";

                            TipSO asset;
                            isExist = File.Exists(path);

                            if (isExist)
                            {
                                asset = AssetDatabase.LoadAssetAtPath<TipSO>(path);
                            }
                            else
                            {
                                asset = ScriptableObject.CreateInstance<TipSO>();
                            }

                            asset.tipID = int.Parse(data[1]);
                            asset.tipText = data[2];

                            if (isExist == false)
                            {
                                AssetDatabase.CreateAsset(asset, path);
                            }

                            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

                            AddressableAssetGroup group = settings.FindGroup("Tip");
                            if(group == null)
                            {
                                group = settings.CreateGroup("Tip", false, false, false, null);
                            }

                            AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path.ToString()), group);

                            entry.SetAddress("TipSO" + data[1].ToString());
                            entry.SetLabel("Tip", true);

                            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

                            AssetDatabase.SaveAssets();

                            EditorUtility.SetDirty(asset);
                        }
                        break;
                    default:
                        break;
                }

                AssetDatabase.SaveAssets();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Error result : " + result);
            }
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