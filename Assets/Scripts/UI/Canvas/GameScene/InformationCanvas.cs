using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvas : BaseCanvas
{
    [SerializeField]
    private Text _waveText = null;
    public Text WaveText => _waveText;

    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;
}
