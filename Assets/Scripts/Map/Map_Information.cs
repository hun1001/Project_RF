using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class Map_Information : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPointParent;

        private Transform[] _spawnPoints => _spawnPointParent.GetComponentsInChildren<Transform>().Where(x => x != _spawnPointParent).ToArray();
        
        public Vector3 RandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex].position;
        }
    }
}
