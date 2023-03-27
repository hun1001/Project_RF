using Item;

public class WaveManager : GameWay_Base
{
    private int _repeatCnt = 0;

    private void Start()
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

    public override void StageClear()
    {
        if (_currentStage < _stageListSO.Stages.Length - 1)
        {
            _currentStage++;
        }
        else
        {
            _repeatCnt++;
        }

        ItemManager.Instance.ItemPickUp();
        Spawn();
    }
}
