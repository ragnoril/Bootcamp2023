using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _enemyPrefab;

        private void Start()
        {
            GameManager.Instance.OnWaveStarted += SpawnNewWave;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnWaveStarted -= SpawnNewWave;
        }

        private void SpawnNewWave(int waveCount)
        {
            for(int i = 0; i < waveCount; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Vector2 displacement = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(3f, 10f);

            Vector3 spawnPosition = new Vector3(transform.position.x + displacement.x, transform.position.y, transform.position.z + displacement.y);

            //Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyController enemy = ObjectPool.Instance.EnemyPool.Get();
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

        }
    }
}