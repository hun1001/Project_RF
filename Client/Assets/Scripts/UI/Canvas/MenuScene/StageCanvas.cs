using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCanvas : BaseCanvas
{
    public void SelectStage(int idx)
    {
        GameWay_Base.CurrentStage = idx;
    }
}
