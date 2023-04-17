using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public void SetIconColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
