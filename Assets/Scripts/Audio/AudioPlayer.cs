using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public float duration;
        public string exposedVolumeName;
        public bool isSfx;
        private bool _isPlaying;

        // Start is called before the first frame update
        void Start()
        {
            var maxVolume = isSfx ? SettingsManager.Instance.sfxVolume : SettingsManager.Instance.musicVolume;
            StartCoroutine(FadeMixerGroup.StartFade(audioMixer, exposedVolumeName, duration, 
                           maxVolume));
        }

        public void FadeOut()
        {
            StartCoroutine(FadeMixerGroup.StartFade
                (
                    audioMixer, 
                    exposedVolumeName, 
                    duration, 
                    0.0001f
                )
            );
        }
    
        public void SetVolume(float volume)
        {
            if (isSfx)
            {
                SettingsManager.Instance.sfxVolume = volume;
            }
            else
            {
                SettingsManager.Instance.musicVolume = volume;
            }
            
            audioMixer.SetFloat(exposedVolumeName, Mathf.Log10(volume) * 20);
        }
    }
}
