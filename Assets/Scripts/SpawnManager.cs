using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Dictionary<GroupType, List<GameObject>> _spawnedUnits = new Dictionary<GroupType, List<GameObject>>();

    public void SpawnUnit(GameObject unit, Vector3 position, Quaternion rotation)
    {

    }
}
