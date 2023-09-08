using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static readonly string _savePath = Application.persistentDataPath + "/save/";
    public static string SavePath => _savePath;

    public static void Save<T>(string key, T data)
    {
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_savePath + key + ".json", json);
    }

    public static T Load<T>(string key)
    {
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        if (!File.Exists(_savePath + key + ".json"))
        {
            return default(T);
        }

        string json = File.ReadAllText(_savePath + key + ".json");
        return JsonUtility.FromJson<T>(json);
    }

    public static void DeleteAllSaveData()
    {
        DirectoryInfo di = new DirectoryInfo(_savePath);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    public static bool WasSaved(string key) => File.Exists(_savePath + key + ".json");
}
