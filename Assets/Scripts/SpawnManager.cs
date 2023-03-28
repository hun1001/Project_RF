using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class SpawnManager : MonoBehaviour
{
    private Dictionary<GroupType, List<CustomObject>> _spawnedUnits = new Dictionary<GroupType, List<CustomObject>>();

    public void SpawnUnit(CustomObject unit, Vector3 position, Quaternion rotation, GroupType groupType)
    {
        var u = PoolManager.Get(unit.ID, position, rotation);
    }
}
