using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Map;

namespace Stage
{
    public class StageManager : GameWay_Base
    {
        protected override void Start()
        {
            StageStart();
        }

        /// <summary> 스테이지가 시작될 때 실행하는 함수 </summary>
        private void StageStart()
        {
            if (_currentMap != null)
            {
                MapRemove();
            }
            _currentMap = RandomMapSelect();
            MapCreation();
            Spawn();
        }

        /// <summary> 그전에 사용한 맵을 지우는 함수 </summary>
        private void MapRemove()
        {
            PoolManager.Instance.Pool(_currentMap.name, _currentMap.gameObject);
            _currentMap = null;
        }

        protected override void StageClear()
        {
            if (_currentStage >= _stageListSO.Stages.Length - 1)
            {
                // 완전 클리어
                // 결과창 띄우고 다음 스테이지 해금
                return;
            }

            // 아이템 창 띄우기
        }

        /// <summary> 다음 스테이지로 넘어갈 때 실행하는 함수 </summary>
        private void NextStage()
        {
            _currentStage++;
            StageStart();
        }
    }
}