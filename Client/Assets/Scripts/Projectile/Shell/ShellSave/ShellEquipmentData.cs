using System;
using System.Collections.Generic;

[Serializable]
public class ShellEquipmentData
{
    public List<string> _shellEquipmentData;

    public ShellEquipmentData()
    {
        _shellEquipmentData = new List<string>(new string[2]);
    }
}
