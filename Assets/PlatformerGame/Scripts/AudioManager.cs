using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Platformer
{
    public class AudioManager : MonoBehaviour
    { 
        private void Awake()
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
            if (PlayerPrefs.GetInt("MusicMuted", 0) == 0)
                IsMusicMuted = false;
            else
                IsMusicMuted = true;

            if (PlayerPrefs.GetInt("SoundMuted", 0) == 0)
                IsSoundMuted = false;
            else
                IsSoundMuted = true;

        }

        [SerializeField]
        private AudioSource _musicPlayer;

        [SerializeField]
        private AudioSource[] _soundChannels;

        [SerializeField]
        private float _musicVolume;

        [SerializeField]
        private float _soundVolume;

        [SerializeField]
        private bool _isMusicMuted;

        [SerializeField]
        private bool _isSoundMuted;

        public bool IsMusicMuted
        {
            get { return _isMusicMuted; }
            set
            {
                _isMusicMuted = value;
                _musicPlayer.mute = _isMusicMuted;
                PlayerPrefs.SetInt("MusicMuted", _isMusicMuted ? 1 : 0);
            }
        }

        public bool IsSoundMuted
        {
            get { return _isSoundMuted; }
            set
            {
                _isSoundMuted = value;
                foreach (var channel in _soundChannels)
                {
                    channel.mute = _isSoundMuted;
                }
                PlayerPrefs.SetInt("SoundMuted", _isSoundMuted ? 1 : 0);
            }
        }

        public float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                _musicPlayer.volume = _musicVolume;
                PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            }
        }

        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                foreach (var channel in _soundChannels)
                {
                    channel.volume = _soundVolume;
                }
                PlayerPrefs.SetFloat("SoundVolume", _soundVolume);
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            _musicPlayer.clip = clip;
            _musicPlayer.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            foreach(var channel in  _soundChannels)
            {
                if (!channel.isPlaying)
                {
                    channel.PlayOneShot(clip);
                    break;
                }
            }
        }

    }
}