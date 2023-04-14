using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoW;

public class Minimap : MonoBehaviour
{
    private FogOfWarMinimap _fogOfWarMinimap = null;

    private void Awake()
    {
        _fogOfWarMinimap = GetComponent<FogOfWarMinimap>();
        _fogOfWarMinimap.camera = GameObject.FindWithTag("MinimapCamera").GetComponent<Camera>();
    }
}
