using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClickActiveItem : Active_Item
{
    public override void ItemEquip(int idx)
    {
        //GearManager.Instance.ControllerCanvas.ButtonGroup.SetButton(idx, ClickEvent, true);
    }

    protected override void ClickEvent()
    {
        Debug.Log("Click!");
    }
}
