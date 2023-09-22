using System.Linq;
using UnityEngine;

namespace Map
{
    public class Map_Information : MonoBehaviour
    {
        /// <summary> 적 생성 위치들의 부모 </summary>
        [SerializeField]
        private Transform _spawnPointParent;

        public Transform[] SpawnPoints => _spawnPointParent.GetComponentsInChildren<Transform>().Where(x => x != _spawnPointParent).ToArray();

        private int _beforeIndex = 0;

        public Vector3 RandomSpawnPoint()
        {
            int index = Random.Range(0, SpawnPoints.Length);

            if (index == _beforeIndex)
            {
                index = (index + 1) % SpawnPoints.Length;
            }

            _beforeIndex = index;

            return SpawnPoints[index].position;
        }
    }
}
