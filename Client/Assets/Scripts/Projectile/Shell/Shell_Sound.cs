using Pool;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Shell_Sound : Shell_Component
{
    public void PlaySound(SoundType soundType, AudioMixerType type = AudioMixerType.Master, float volume = 1f)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Instance.transform.position, Quaternion.identity);
        audioSource.SetSound((Instance as Shell).ShellSound.GetAudioClip(soundType));
        audioSource.SetGroup(type);
        audioSource.SetVolume(volume);
        audioSource.SetDimensionSound(0f);
        audioSource.Play();
    }
}
