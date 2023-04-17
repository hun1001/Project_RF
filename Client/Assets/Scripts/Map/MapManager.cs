using System.Collections;
using Map;
using UnityEngine;
using Util;

public class MapManager : MonoSingleton<MapManager>
{
    public MapSO MapData => FindObjectOfType<Map_Information>().MapData;
}
