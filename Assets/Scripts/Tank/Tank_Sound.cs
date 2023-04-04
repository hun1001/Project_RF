using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Sound : Tank_Component
{
    private AudioSourceController _engineSource;
    private AudioSourceController _trackSource;

    public void MoveSoundUpdate(float speedPercent)
    {

    }

    public void StartEngineSound()
    {
        _engineSource = PlaySound(SoundType.Engine, AudioMixerType.Sfx, 0.7f, true);

        _trackSource = PlaySound(SoundType.Track, AudioMixerType.Sfx, 0f, true);
    }

    public AudioSourceController PlaySound(SoundType soundType, AudioMixerType type = AudioMixerType.Master, float volume = 1f, bool isLoop = false)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Instance.transform.position, Quaternion.identity, Instance.transform);
        audioSource.SetSound((Instance as Tank).TankSound.GetAudioClip(soundType));
        audioSource.SetGroup(type);
        audioSource.SetVolume(volume);
        if (isLoop) audioSource.SetLoop();
        audioSource.Play();

        return audioSource;
    }
}
