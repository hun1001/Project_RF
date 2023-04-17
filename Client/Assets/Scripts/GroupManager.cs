using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using FoW;

public class GroupManager : MonoSingleton<GroupManager>
{
    [SerializeField]
    private List<FogOfWarTeam> _groupList = new List<FogOfWarTeam>();
}
