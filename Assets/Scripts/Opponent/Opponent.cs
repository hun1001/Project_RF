using UnityEngine;
using Base;
using System.Linq;
using Random = UnityEngine.Random;

namespace Opponent
{
    public class Opponent : CustomGameObject<Opponent>
    {
        [SerializeField]
        private Transform _spawnPointParent = null;

        public Transform[] SpawnPoints => _spawnPointParent.GetComponentsInChildren<Transform>().Where(x => x != _spawnPointParent).ToArray();
        public Transform GetRandomSpawnPoint => SpawnPoints[Random.Range(0, SpawnPoints.Length)];

        [SerializeField]
        private SO.OpponentSO _opponentSO = null;
        public SO.OpponentSO OpponentSO => _opponentSO;

        [SerializeField]
        private UI.TextController _waveTimer = null;
        public UI.TextController WaveTimer => _waveTimer;
    }
}
