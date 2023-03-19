using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Map;

namespace Stage
{
    public class Stage_Spawn : MonoBehaviour
    {
        [SerializeField]
        private StageListSO _stageListSO = null;

        private int _currentStage = 0;
        private Map_Information _currentMap = null;

        private void StageStart()
        {
            _currentMap = RandomMapSelect();
            Spawn();
        }

        private void Spawn()
        {
            for(int i = 0; i < _stageListSO.Stages[_currentStage].Enemys.Length; i++)
            {
                GameObject enemy = PoolManager.Get(_stageListSO.Stages[_currentStage].Enemys[i].name);
                enemy.transform.position = _currentMap.RandomSpawnPoint();
            }
        }

        private Map_Information RandomMapSelect()
        {
            int randomIndex = Random.Range(0, _stageListSO.Maps.Length);
            return _stageListSO.Maps[randomIndex];
        }
    }
}