using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectCanvas : BaseCanvas
{
    [SerializeField]
    private UpgradeSelectHandle _upgradeSelectHandle = null;

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        SetUpgradeSelectNode();
    }

    private void SetUpgradeSelectNode()
    {
        _upgradeSelectHandle.Setting();
    }
}
