using Event;
using System.Collections.Generic;

public static class ShellSaveManager
{
    private static Dictionary<string, ShellEquipmentData> _shellEquipmentDataDict = new Dictionary<string, ShellEquipmentData>();

    public static ShellEquipmentData GetShellEquipment(string tankID)
    {
        ShellEquipmentData shellEquipmentData = null;
        if (_shellEquipmentDataDict.TryGetValue(tankID, out shellEquipmentData) == false)
        {
            shellEquipmentData = new ShellEquipmentData();
            _shellEquipmentDataDict.Add(tankID, shellEquipmentData);
        }

        if (SaveManager.WasSaved(SaveKey.GetShellEquipment(tankID)))
        {
            _shellEquipmentDataDict[tankID] = SaveManager.Load<ShellEquipmentData>(SaveKey.GetShellEquipment(tankID));
        }
        else
        {
            SaveShellEquipment(tankID);
        }

        return _shellEquipmentDataDict[tankID];
    }

    public static void ShellEquip(string tankID, int idx, string shellName)
    {
        if (!_shellEquipmentDataDict.ContainsKey(tankID))
        {
            _shellEquipmentDataDict.Add(tankID, new ShellEquipmentData());
        }
        
        _shellEquipmentDataDict[tankID]._shellEquipmentList[idx] = shellName;

        SaveShellEquipment(tankID);
        EventManager.TriggerEvent(EventKeyword.ShellReplacement);
    }

    public static void SaveShellEquipment(string tankID)
    {
        SaveManager.Save(SaveKey.GetShellEquipment(tankID), _shellEquipmentDataDict[tankID]);
    }
}
