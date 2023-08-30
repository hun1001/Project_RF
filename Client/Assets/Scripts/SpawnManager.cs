using Util;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    public Tank SpawnUnit(string unitID, Vector3 position, Quaternion rotation, GroupType groupType, BaseSubArmament subArmament = null)
    {
        var spawnedUnit = PoolManager.Get<Tank>(unitID, position, rotation).SetTank(groupType, subArmament);

        return spawnedUnit;
    }
}
