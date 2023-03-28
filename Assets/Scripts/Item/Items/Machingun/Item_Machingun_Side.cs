using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;

public class Item_Machingun_Side : Item_Machingun
{
    private static int _sideMachingun = 1;

    private Item_Base _otherSideMachingun = null;

    protected override void SetPosAndRot()
    {
        // Right
        if (_sideMachingun % 2 == 1)
        {
            _sideMachingun++;
            transform.localPosition = new Vector3(3f, 0f, -2f);

            _otherSideMachingun = PoolManager.Get<Item_Base>("Side_Machingun", ItemManager.Instance.PlayerTank);
            ItemManager.Instance.HaveItemList.Add(_otherSideMachingun, 0);
            _otherSideMachingun.AddItem();
        }
        // Left
        else
        {
            _sideMachingun++;
            transform.localPosition = new Vector3(-3f, 0f, -2f);
        }
        base.SetPosAndRot();
    }

    protected override void UpgradeItem()
    {
        base.UpgradeItem();
        _otherSideMachingun?.SendMessage("UpgradeItem");
    }
}
