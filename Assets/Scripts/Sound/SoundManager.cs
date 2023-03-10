using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Util;

namespace Sound
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField]
        private AudioMixer _audioMixer = null;
        [SerializeField]
        private AudioClip _ambientSound = null;

        private void Awake()
        {
            LoopPlaySound(_ambientSound, SoundType.BGM);
        }

        public AudioMixerGroup GetAudioMixerGroup(SoundType soundType) => soundType switch
        {
            SoundType.BGM => _audioMixer.FindMatchingGroups("BGM")[0],
            SoundType.SFX => _audioMixer.FindMatchingGroups("SFX")[0],
            _ => null
        };
        
        public void PlaySound(AudioClip audioClip, SoundType soundType, float volume = 1f, float pitch = 1f)
        {
            var audioSource = PoolManager.Instance.Get<Sound>("Assets/Prefabs/Sound/Sound.prefab");
            audioSource.Play(audioClip, GetAudioMixerGroup(soundType), volume, pitch);
        }

        public Sound LoopPlaySound(AudioClip audioClip, SoundType soundType, float volume = 0.5f, float pitch = 1f)
        {
            var audioSource = PoolManager.Instance.Get<Sound>("Assets/Prefabs/Sound/Sound.prefab");
            audioSource.LoopPlay(audioClip, GetAudioMixerGroup(soundType), volume, pitch);
            return audioSource;
        }
    }
}
