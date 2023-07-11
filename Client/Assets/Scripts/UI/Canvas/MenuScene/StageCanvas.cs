using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageCanvas : BaseCanvas
{
    [SerializeField]
    private Button[] _selectStageButtons = null;

    private void Awake()
    {
        for (int i = 0; i < _selectStageButtons.Length; i++)
        {
            _selectStageButtons[i].onClick.AddListener(() => SelectStage(i));
        }
    }

    public void SelectStage(int idx)
    {
        GameWay_Base.CurrentStage = idx;
        PlayButtonSound();
    }
}
