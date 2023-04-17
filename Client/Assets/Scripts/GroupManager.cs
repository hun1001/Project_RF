using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using FoW;

public class GroupManager : MonoSingleton<GroupManager>
{
    [SerializeField]
    private Dictionary<GroupType, Color> _groupColorDictionary = new Dictionary<GroupType, Color>();
    public Dictionary<GroupType, Color> GroupColorList => _groupColorDictionary;

    public void SetGroupColorList(List<Color> groupColorList)
    {
        _groupColorDictionary.Clear();
        for (int i = 0; i < groupColorList.Count; i++)
        {
            _groupColorDictionary.Add((GroupType)i + 1, groupColorList[i]);
        }

        foreach (var groupColor in _groupColorDictionary)
        {
            Debug.Log($"GroupColor_{groupColor.Key}_{groupColor.Value}");
        }
    }
}
