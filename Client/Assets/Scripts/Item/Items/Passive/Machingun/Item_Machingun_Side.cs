using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;

public class Item_Machingun_Side : Item_Machingun
{
    private static int _sideMachingun = 1;

    private Passive_Item _otherSideMachingun = null;

    protected override void SetPosAndRot()
    {
        // Right
        if (_sideMachingun % 2 == 1)
        {
            _sideMachingun++;
            transform.localPosition = new Vector3(3f, 0f, -2f);

            _otherSideMachingun = PoolManager.Get<Passive_Item>(ID, GearManager.Instance.Player);

            _otherSideMachingun.ItemEquip();
        }
        // Left
        else
        {
            _sideMachingun++;
            transform.localPosition = new Vector3(-3f, 0f, -2f);
        }
        base.SetPosAndRot();
    }
}
