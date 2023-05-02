using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoW;

public class MinimapIcon : MonoBehaviour
{
    public void SetIconColor(GroupType g)
    {
        GetComponent<SpriteRenderer>().color = GroupManager.Instance.GroupColorList[(int)g];
        //GetComponent<HideInFog>()?.team = (int)g;
    }
}
