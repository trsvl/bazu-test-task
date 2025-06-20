using System;
using _Project.Scripts.Enemies.Types;
using _Project.Scripts.Utils.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
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

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), 0f, _timeBetweenSpawns);
        }

        public void Spawn()
        {
            var spawnPoint = GetRandomSpawnPoint();
            Instantiate(GetRandomEnemyPrefab(), spawnPoint.position, spawnPoint.rotation);
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