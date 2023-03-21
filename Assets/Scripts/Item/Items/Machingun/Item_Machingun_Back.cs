using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Machingun_Back : Item_Machingun
{
    protected override void SetPosAndRot()
    {
        transform.localPosition = new Vector3(0, -4f, -2f);
        transform.forward = Vector3.down;
    }
}
