using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Map")]
public class MapSO : ScriptableObject
{
    [SerializeField]
    private Vector2Int _mapResolution = Vector2Int.zero;
    public Vector2Int MapResolution => _mapResolution;

    [SerializeField]
    private float _mapSize = 0;
    public float MapSize => _mapSize;

    [SerializeField]
    private Vector2 _mapOffset = Vector2.zero;
    public Vector2 MapOffset => _mapOffset;
}
