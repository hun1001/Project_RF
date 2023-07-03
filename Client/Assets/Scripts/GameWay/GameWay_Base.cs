using Event;
using Map;
using Pool;
using Stage;
using UnityEngine;
using Util;

public abstract class GameWay_Base : MonoSingleton<GameWay_Base>
{
    /// <summary> 해당 스테이지 리스트의 SO </summary>
    [SerializeField]
    protected StageListSO _stageListSO = null;
    public StageListSO StageListSO => _stageListSO;

    /// <summary> 현재 스테이지 </summary>
    public static int CurrentStage = 0;
    /// <summary> 현재 맵 정보 </summary>
    protected static Map_Information _currentMap = null;

    [HideInInspector]
    public static int RemainingEnemy = 0;

    protected virtual void Awake()
    {
        EventManager.DeleteEvent(EventKeyword.EnemyDie);
        EventManager.StartListening(EventKeyword.EnemyDie, EnemyDie);
    }

    private void EnemyDie()
    {
        RemainingEnemy--;
        if (RemainingEnemy <= 0)
        {
            StageClear();
        }
    }

    /// <summary> 적을 생성할 때 사용하는 함수 </summary>
    protected virtual void Spawn()
    {
        for (int i = 0; i < _stageListSO.Stages[CurrentStage].Enemys.Length; i++)
        {
            var ai = PoolManager.Get<TankAI>("AI", _currentMap.SpawnPoints[i].position, Quaternion.identity);
            ai.Init(_stageListSO.Stages[CurrentStage].Enemys[i].ID);
        }
        RemainingEnemy += _stageListSO.Stages[CurrentStage].Enemys.Length;
    }

    /// <summary> 맵을 랜덤으로 선택하는 함수 </summary>
    /// <returns> 랜덤으로 선택된 맵 </returns>
    protected Map_Information RandomMapSelect()
    {
        if(_stageListSO.Maps.Length == 0) return FindObjectOfType<Map_Information>();
        int randomIndex = Random.Range(0, _stageListSO.Maps.Length);
        return _stageListSO.Maps[randomIndex];
    }

    /// <summary> 선택된 맵을 생성하는 함수 </summary>
    protected void MapCreation()
    {
        if (_stageListSO.Maps.Length == 0) return;
        _currentMap = PoolManager.Get(_currentMap.name).GetComponent<Map_Information>();
    }

    /// <summary> 해당 스테이지를 클리어시 실행하는 함수 </summary>
    public abstract void StageClear();
}
