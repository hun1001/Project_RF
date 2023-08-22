using Item;
using Event;

public class WaveManager : GameWay_Base
{
    private int _repeatCnt = 0;

    private void Start()
    {
        _currentMap = RandomMapSelect();
        // MapCreation();
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
        if (CurrentStage < _stageListSO.Stages.Length - 1)
        {
            CurrentStage++;
        }
        else
        {
            _repeatCnt++;
        }

        EventManager.TriggerEvent("Clear");
        // ItemManager.Instance.ItemPickUp();
        Spawn();
    }
}
