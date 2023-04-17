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
}
