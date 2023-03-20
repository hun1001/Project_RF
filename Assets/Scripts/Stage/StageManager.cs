using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Map;

namespace Stage
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField]
        private StageListSO _stageListSO = null;

        private static int _currentStage = 0;
        private static Map_Information _currentMap = null;

        private void Start()
        {
            StageStart();
        }

        private void StageStart()
        {
            if(_currentMap != null)
            {
                MapRemove();
            }
            _currentMap = RandomMapSelect();
            MapCreation();
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

        private void MapCreation()
        {
            _currentMap = PoolManager.Get(_currentMap.name).GetComponent<Map_Information>();
            

        }

        private void MapRemove()
        {
            PoolManager.Pool(_currentMap.name, _currentMap.gameObject);
            _currentMap = null;
        }

        private void StageClear()
        {
            if(_currentStage >= _stageListSO.Stages.Length - 1)
            {
                // 완전 클리어
                // 결과창 띄우고 다음 스테이지 해금
                return;
            }

            // 아이템 창 띄우기
        }

        private void NextStage()
        {
            _currentStage++;
            StageStart();
        }
    }
}