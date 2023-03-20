using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class Map_Information : MonoBehaviour
    {
        /// <summary> 적 생성 위치들의 부모 </summary>
        [SerializeField]
        private Transform _spawnPointParent;

        /// <summary> 적 생성 위치들 </summary>
        private Transform[] _spawnPoints => _spawnPointParent.GetComponentsInChildren<Transform>().Where(x => x != _spawnPointParent).ToArray();
        
        /// <summary> 적 생성 위치를 랜덤으로 반환하는 함수 </summary>
        /// <returns> 적 생성 위치 </returns>
        public Vector3 RandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex].position;
        }
    }
}
