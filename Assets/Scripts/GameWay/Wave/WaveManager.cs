using Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaveManager : StageManager
{
    private int _repeatCnt = 0;

    protected override void Start()
    {
        _currentMap = RandomMapSelect();
        MapCreation();
        Spawn();
    }

    protected override void Spawn()
    {
        for(int i = 0; i < (_repeatCnt * 0.25) + 1; i++)
        {
            base.Spawn();
        }
    }

    protected override void StageClear()
    {
        if (_currentStage < _stageListSO.Stages.Length - 1)
        {
            _currentStage++;
        }
        else
        {
            _repeatCnt++;
        }

        // 아이템 창 띄우고
        Spawn();
    }
}
