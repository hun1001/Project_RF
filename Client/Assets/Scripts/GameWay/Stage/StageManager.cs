using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Map;
using Event;

namespace Stage
{
    public class StageManager : GameWay_Base
    {
        private void Awake()
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
            if (_currentMap == null) return;
            PoolManager.Pool(_currentMap.name, _currentMap.gameObject);
            _currentMap = null;
        }

        public override void StageClear()
        {
            EventManager.TriggerEvent(EventKeyword.StageClear);
        }
    }
}