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

        // 스테이지가 시작될 때 실행하는 함수
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

        // 적을 생성할 때 사용하는 함수
        private void Spawn()
        {
            for(int i = 0; i < _stageListSO.Stages[_currentStage].Enemys.Length; i++)
            {
                GameObject enemy = PoolManager.Get(_stageListSO.Stages[_currentStage].Enemys[i].name);
                enemy.transform.position = _currentMap.RandomSpawnPoint();
            }
        }

        // 맵을 랜덤으로 선택하는 함수
        private Map_Information RandomMapSelect()
        {
            int randomIndex = Random.Range(0, _stageListSO.Maps.Length);
            return _stageListSO.Maps[randomIndex];
        }

        // 선택된 맵을 생성하는 함수
        private void MapCreation()
        {
            _currentMap = PoolManager.Get(_currentMap.name).GetComponent<Map_Information>();
            

        }

        // 그전에 사용한 맵을 지우는 함수
        private void MapRemove()
        {
            PoolManager.Pool(_currentMap.name, _currentMap.gameObject);
            _currentMap = null;
        }

        // 해당 스테이지를 클리어시 실행하는 함수
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

        // 다음 스테이지로 넘어갈 때 실행하는 함수
        private void NextStage()
        {
            _currentStage++;
            StageStart();
        }
    }
}