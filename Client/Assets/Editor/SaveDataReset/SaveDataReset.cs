using UnityEditor;
using System.IO;
using UnityEngine;

public class SaveDataReset
{
    [MenuItem("Tools/SaveDataReset")]
    static void Init()
    {
        PlayerPrefs.DeleteAll();

        DirectoryInfo di = new DirectoryInfo(SaveManager.SavePath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }
}
