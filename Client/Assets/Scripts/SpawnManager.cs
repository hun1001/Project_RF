using Util;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    public Tank SpawnUnit(string unitID, Vector3 position, Quaternion rotation, GroupType groupType)
    {
        var spawnedUnit = PoolManager.Get<Tank>(unitID, position, rotation).SetTank(groupType);

        var minimapIcon = PoolManager.Get<MinimapIcon>("Assets/Prefabs/MinimapIcon.prefab", spawnedUnit.transform);
        minimapIcon.transform.localPosition = new Vector3(0, 0, -15);
        minimapIcon.SetIconColor(groupType);

        return spawnedUnit;
    }
}
