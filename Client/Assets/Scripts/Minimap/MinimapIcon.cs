using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public void SetIconColor(GroupType g)
    {
        GetComponent<SpriteRenderer>().color = GroupManager.Instance.GroupColorList[(int)g];
    }
}
