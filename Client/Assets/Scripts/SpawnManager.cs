using Util;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using FoW;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    // private Dictionary<GroupType, List<CustomObject>> _spawnedUnits = new Dictionary<GroupType, List<CustomObject>>();

    public Tank SpawnUnit(string unitID, Vector3 position, Quaternion rotation, GroupType groupType)
    {
        var spawnedUnit = PoolManager.Get<Tank>(unitID, position, rotation).SetTank(groupType);

        var minimapIcon = PoolManager.Get<MinimapIcon>("Assets/Prefabs/MinimapIcon.prefab", spawnedUnit.transform);
        // minimapIcon.SetIconColor(GroupManager.Instance.GroupColorList[groupType]);

        return spawnedUnit;
    }
}
