using Event;
using Map;
using Pool;
using Stage;
using UnityEngine;
using Util;
using UnityEngine.SceneManagement;

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

    public static int RemainingEnemy = 0;

    private void Awake()
    {
        EventManager.StartListening(EventKeyword.EnemyDie, EnemyDie);

        SceneManager.sceneLoaded += (_, _) => GameWayReset();
    }

    private void GameWayReset()
    {
        CurrentStage = 0;
        RemainingEnemy = 0;
        _currentMap = FindObjectOfType<Map_Information>();
    }

    private void EnemyDie()
    {
        RemainingEnemy--;
        if (RemainingEnemy <= 0)
        {
            StageClear();
        }
    }

    protected virtual void Spawn()
    {
        for (int i = 0; i < _stageListSO.Stages[CurrentStage].Enemys.Length; i++)
        {
            var ai = PoolManager.Get<TankAI>("AI", _currentMap.RandomSpawnPoint(), Quaternion.identity);
            ai.Init(_stageListSO.Stages[CurrentStage].Enemys[i].ID);
        }
        RemainingEnemy = _stageListSO.Stages[CurrentStage].Enemys.Length;
    }

    public abstract void StageClear();
}
