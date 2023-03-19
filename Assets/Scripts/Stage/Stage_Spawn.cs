using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

namespace Stage
{
    public class Stage_Spawn : MonoBehaviour
    {
        [SerializeField]
        private StageListSO _stageListSO;

        private int _currentStage = 0;
        private GameObject _currentMap;

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
                //enemy.transform.position =
            }
        }

        private GameObject RandomMapSelect()
        {
            int randomIndex = Random.Range(0, _stageListSO.Maps.Length);
            return _stageListSO.Maps[randomIndex];
        }
    }
}