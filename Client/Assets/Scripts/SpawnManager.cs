using Util;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    private Action<Tank> _onSpawnedEnemyUnit;
    public void AddOnSpawnedEnemyUnitAction(Action<Tank> action)
    {
        _onSpawnedEnemyUnit += action;
    }
    public void ResetOnSpawnEnemyUnitAction() => _onSpawnedEnemyUnit = null;

    public Tank SpawnUnit(string unitID, Vector3 position, Quaternion rotation, GroupType groupType, BaseSubArmament subArmament = null)
    {
        var spawnedUnit = PoolManager.Get<Tank>(unitID, position, rotation).SetTank(groupType, subArmament);

        if (groupType == GroupType.Enemy)
        {
            _onSpawnedEnemyUnit?.Invoke(spawnedUnit);
        }

        return spawnedUnit;
    }
}
