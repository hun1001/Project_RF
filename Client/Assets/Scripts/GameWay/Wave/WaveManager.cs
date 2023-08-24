﻿using Item;
using Event;
using Pool;
using UnityEngine;

public class WaveManager : GameWay_Base
{
    //private int _repeatCnt = 0;

    private void Start()
    {
        Spawn();
    }

    protected override void Spawn()
    {
        if (StageListSO.Stages[CurrentStage].IsBoss)
        {
            var boss = PoolManager.Get<TankAI>("Boss_AI", _currentMap.RandomSpawnPoint(), Quaternion.identity);
            RemainingEnemy = _stageListSO.Stages[CurrentStage].Enemys.Length;
        }
        else
        {
            base.Spawn();
        }
    }

    public override void StageClear()
    {
        
        EventManager.TriggerEvent("Clear");
        
        if(CurrentStage + 1 >= StageListSO.Stages.Length)
        {
            EventManager.TriggerEvent(EventKeyword.StageClear);
            return;
        }

        CurrentStage++;
        Spawn();
    }
}
