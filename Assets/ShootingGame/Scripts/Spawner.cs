using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    private float _spawnTimer = 0f;
    [SerializeField] private float _spawnRate;


    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnRate)
        {
            SpawnEnemy();
            _spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 displacement = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(3f, 10f);

        Vector3 spawnPosition = new Vector3(transform.position.x + displacement.x, transform.position.y, transform.position.z + displacement.y);

        Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

    }
}
