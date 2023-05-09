using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvas : BaseCanvas
{
    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;
}
