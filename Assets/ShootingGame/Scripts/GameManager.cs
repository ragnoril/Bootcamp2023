using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShooterGame
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance == this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public event Action OnGameStart;

        public event Action<EnemyController> OnEnemyHit;
        public event Action OnEnemyKilled;

        public event Action<int> OnPlayerHit;
        public event Action OnPlayerKilled;
        public event Action<int> OnScoreChanged;

        public event Action<int> OnWaveStarted;

        private int _waveCount;
        private float _waveTimer;
        [SerializeField]
        private float _waveRate;


        [SerializeField]
        private PlayerController Player;


        private void Start()
        {
            _waveCount = 0;
            _waveTimer = _waveRate;
            GameStarted();
        }

        private void Update()
        {
            _waveTimer += Time.deltaTime;

            if (_waveTimer > _waveRate)
            {
                StartNextWave();
            }
        }

        private void StartNextWave()
        {
            _waveCount += 1;
            _waveTimer = 0;
            _waveRate *= 1.1f;

            OnWaveStarted?.Invoke(_waveCount);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        public void GameStarted()
        {
            OnGameStart?.Invoke();
        }

        public void EnemyHit(EnemyController enemy)
        {
            OnEnemyHit?.Invoke(enemy);
        }

        public void EnemyKilled()
        {
            OnEnemyKilled?.Invoke();
        }

        public void PlayerHit(int damage)
        {
            OnPlayerHit?.Invoke(damage);
        }

        public void PlayerKilled()
        {
            OnPlayerKilled?.Invoke();
        }

        public void ScoreChanged(int lastScore)
        {
            OnScoreChanged?.Invoke(lastScore);
        }

    }
}