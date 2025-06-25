using _Project.Scripts.Enemies.Types;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemies
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private float _timeBetweenSpawns;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Enemy[] _enemies;
        public Transform[] SpawnPoints => _spawnPoints;

        public static EnemySpawner Instance;


        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
        }

        public void StartSpawn()
        {
            if (!IsServer) return;

            InvokeRepeating(nameof(SpawnRpc), 0f, _timeBetweenSpawns);
        }

        [Rpc(SendTo.Server)]
        private void SpawnRpc()
        {
            var spawnPoint = GetRandomSpawnPoint();
            var enemy = Instantiate(GetRandomEnemyPrefab(), spawnPoint.position, spawnPoint.rotation);
            enemy.GetComponent<NetworkObject>().Spawn();
        }

        private Enemy GetRandomEnemyPrefab()
        {
            int randomIndex = Random.Range(0, _enemies.Length);
            return _enemies[randomIndex];
        }

        private Transform GetRandomSpawnPoint()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[randomIndex];
        }
    }
}