using System;
using System.Collections.Generic;

[Serializable]
public class ShellEquipmentData
{
    public List<string> _shellEquipmentList;

    public ShellEquipmentData()
    {
        _shellEquipmentList = new List<string>(new string[2]);
        _shellEquipmentList[0] = "";
        _shellEquipmentList[1] = "";
    }
}
