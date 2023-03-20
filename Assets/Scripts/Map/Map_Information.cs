using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class Map_Information : MonoBehaviour
    {
        // 적 생성 위치들의 부모
        [SerializeField]
        private Transform _spawnPointParent;

        // 적 생성 위치들
        private Transform[] _spawnPoints => _spawnPointParent.GetComponentsInChildren<Transform>().Where(x => x != _spawnPointParent).ToArray();
        
        // 적 생성 위치를 랜덤으로 반환하는 함수
        public Vector3 RandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex].position;
        }
    }
}
