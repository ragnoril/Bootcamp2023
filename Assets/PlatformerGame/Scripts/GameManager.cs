using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _levelPrefabs;
        private Level _currentLevel;
        public Level CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            private set
            {
                _currentLevel = value;
            }

        }
        [SerializeField]
        private int _currentLevelId;

        [SerializeField]
        private PlayerController _player;

        [SerializeField]
        private UISettings _uiSettings;

        public AudioManager Audio;

        public event Action OnLevelStarted;
        public event Action OnLevelCompleted;

        private void Start()
        {
            _uiSettings.Init(this);
            _currentLevelId = PlayerPrefs.GetInt("CurrentLevel", 0);
            _player.Init(this);
            StartLevel();
        }

        private void StartLevel()
        {
            GameObject goLevel = Instantiate(_levelPrefabs[_currentLevelId % _levelPrefabs.Length], Vector3.zero, Quaternion.identity);
            _currentLevel = goLevel.GetComponent<Level>();
            _currentLevel.Init(this);
            OnLevelStarted?.Invoke();
        }

        public void CheckIfLevelEnded()
        {
            int collectedAmount = _player.Data.CoinsCollected;
            int neededAmount = _currentLevel.MinimumCoinCollectAmount;

            if (collectedAmount >= neededAmount)
            {
                OnLevelCompleted?.Invoke();
            }
        }

        public void GotoNextLevel()
        {
            _currentLevelId += 1;
            PlayerPrefs.SetInt("CurrentLevel", _currentLevelId);
            Destroy(_currentLevel.gameObject);
            StartLevel();
        }
    }
}