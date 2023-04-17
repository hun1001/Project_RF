using Util;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using FoW;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    private Dictionary<GroupType, List<CustomObject>> _spawnedUnits = new Dictionary<GroupType, List<CustomObject>>();
    private Dictionary<GroupType, FogOfWarTeam> _groupDictionary = new Dictionary<GroupType, FogOfWarTeam>();

    public Tank SpawnUnit(string unitID, Vector3 position, Quaternion rotation, GroupType groupType)
    {
        if (_groupDictionary.ContainsKey(groupType) == false)
        {
            CreateTeamGroup(groupType);
        }

        var spawnedUnit = PoolManager.Get<Tank>(unitID, position, rotation).SetTank(groupType);
        return spawnedUnit;
    }

    private void CreateTeamGroup(GroupType groupType)
    {
        var team = PoolManager.Get<FogOfWarTeam>("TeamGroup", Vector3.zero, Quaternion.identity);

        team.team = (int)groupType;

        // team의 맵 크기에 맞게 세팅해주는 코드 필요
        MapSO mapData = MapManager.Instance.MapData;
        team.mapSize = mapData.MapSize;
        team.mapResolution = mapData.MapResolution;
        team.mapOffset = mapData.MapOffset;

        _groupDictionary.Add(groupType, team);
    }
}
