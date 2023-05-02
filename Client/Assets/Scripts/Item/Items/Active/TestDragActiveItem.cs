using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDragActiveItem : Active_Item
{
    public override void ItemEquip(int idx)
    {
        _idx = idx;
        GearManager.Instance.ControllerCanvas.ButtonGroup.SetDragButton(idx, () => DragEvent(), true);
    }

    protected override void DragEvent()
    {
        Debug.Log("Drag!" + GearManager.Instance.ControllerCanvas.ButtonGroup.Joysticks[0].Direction);
    }
}
