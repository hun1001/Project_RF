using Item;
using Event;
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
        //for (int i = 0; i < (_repeatCnt * 0.25) + 1; i++)
        if (StageListSO.Stages[CurrentStage].IsBoss)
        {
            // 보스 웨이브 전용 로직 작동
        }
        else
        {
            
        }

        base.Spawn();
    }

    public override void StageClear()
    {
        
        EventManager.TriggerEvent("Clear");
        
        if(CurrentStage >= StageListSO.Stages.Length)
        {
            EventManager.TriggerEvent(EventKeyword.StageClear);
            return;
        }

        CurrentStage++;
        Spawn();
    }
}
