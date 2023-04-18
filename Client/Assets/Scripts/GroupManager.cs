using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using FoW;

public class GroupManager : MonoSingleton<GroupManager>
{
    [SerializeField]
    private List<Color> _groupColorList = new List<Color>();
    public List<Color> GroupColorList
    {
        get => _groupColorList;
        set => _groupColorList = value;
    }

    public Color GetGroupColor(GroupType groupType)
    {
        if (groupType == GroupType.None)
        {
            return Color.white;
        }
        else
        {
            return _groupColorList[(int)groupType];
        }
    }
}
