using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveDataReset
{
    [MenuItem("Tools/SaveDataReset")]
    static void Init()
    {
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
