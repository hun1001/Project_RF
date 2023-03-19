using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Map_Information : MonoBehaviour
    {
        [SerializeField]
        private Vector3[] _spawnPoints;

        public Vector3 RandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex];
        }
    }
}
