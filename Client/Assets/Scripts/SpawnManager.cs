using Util;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using FoW;

public class SpawnManager// : Singleton<SpawnManager>
{
    private Dictionary<GroupType, List<CustomObject>> _spawnedUnits = new Dictionary<GroupType, List<CustomObject>>();
    private Dictionary<GroupType, FogOfWarTeam> _groupDictionary = new Dictionary<GroupType, FogOfWarTeam>();

    public Tank SpawnUnit(CustomObject unit, Vector3 position, Quaternion rotation, GroupType groupType)
    {
        if (_groupDictionary.ContainsKey(groupType) == false)
        {
            CreateTeamGroup(groupType);
        }

        var spawnedUnit = PoolManager.Get<Tank>(unit.ID, position, rotation).SetTank(groupType);
        return spawnedUnit;
    }

    private void CreateTeamGroup(GroupType groupType)
    {
        var team = PoolManager.Get<FogOfWarTeam>("TeamGroup", Vector3.zero, Quaternion.identity);

        team.team = (int)groupType;

        // team의 맵 크기에 맞게 세팅해주는 코드 필요

        _groupDictionary.Add(groupType, team);
    }
}
