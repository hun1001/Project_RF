using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerUtil
{
    public static void SetGameObjectLayer(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameObjectLayer(child.gameObject, layer);
        }
    }
}
