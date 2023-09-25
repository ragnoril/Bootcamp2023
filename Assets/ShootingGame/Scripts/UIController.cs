using ShooterGame;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShooterGame
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _scoreText;

        [SerializeField]
        private TMP_Text _waveText;

        [SerializeField]
        private GameObject _endGamePanel;


        private void Start()
        {
            _endGamePanel.SetActive(false);

            GameManager.Instance.OnScoreChanged += UpdateScoreText;
            GameManager.Instance.OnWaveStarted += UpdateWaveText;
            GameManager.Instance.OnPlayerKilled += GameOver;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnScoreChanged -= UpdateScoreText;
            GameManager.Instance.OnWaveStarted -= UpdateWaveText;
            GameManager.Instance.OnPlayerKilled -= GameOver;
        }

        private void GameOver()
        {
            _endGamePanel.SetActive(true);
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void UpdateWaveText(int wave)
        {
            _waveText.text = wave.ToString();
        }

        public void RestartGame()
        {
            GameManager.Instance.RestartGame();
        }


    }
}