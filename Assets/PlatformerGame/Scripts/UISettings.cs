using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Platformer
{
    public class UISettings : CustomBehaviour
    {
        public Toggle MuteMusicToggle;
        public Toggle MuteSfxToggle;

        public Slider MusicVolumeSlider;
        public Slider SfxVolumeSlider;

        public GameObject OptionsPanel;
        public GameObject LevelCompletedPanel;

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            if (_gameManager.Audio.IsMusicMuted)
            {
                MuteMusicToggle.isOn = true;
            }
            else
            {
                MuteMusicToggle.isOn = false;
            }

            if (_gameManager.Audio.IsSoundMuted)
            {
                MuteSfxToggle.isOn = true;
            }
            else
            {
                MuteSfxToggle.isOn = false;
            }

            MusicVolumeSlider.value = _gameManager.Audio.MusicVolume;
            SfxVolumeSlider.value = _gameManager.Audio.SoundVolume;

            MusicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolumeChange(); });
            SfxVolumeSlider.onValueChanged.AddListener(delegate { SfxVolumeChange(); });

            MuteMusicToggle.onValueChanged.AddListener(delegate { MuteMusic(); });
            MuteSfxToggle.onValueChanged.AddListener(delegate { MuteSfx(); });

            OptionsPanel.SetActive(false);
            LevelCompletedPanel.SetActive(false);

            _gameManager.OnLevelCompleted += OpenLevelFinishPanel;
        }
        private void OnDestroy()
        {
            _gameManager.OnLevelCompleted -= OpenLevelFinishPanel;
        }

        private void OpenLevelFinishPanel()
        {
            LevelCompletedPanel.SetActive(true);
        }

        private void MuteSfx()
        {
            _gameManager.Audio.IsSoundMuted = MuteSfxToggle.isOn;
        }

        private void MuteMusic()
        {
            _gameManager.Audio.IsMusicMuted = MuteMusicToggle.isOn;
        }

        public void MusicVolumeChange()
        {
            _gameManager.Audio.MusicVolume = MusicVolumeSlider.value;
        }

        public void SfxVolumeChange()
        {
            _gameManager.Audio.SoundVolume = SfxVolumeSlider.value;
        }

        public void GotoNextLevel()
        {
            _gameManager.GotoNextLevel();
            LevelCompletedPanel.SetActive(false);
        }

    }
}
