using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Sound : Tank_Component
{
    private AudioSourceController _engineSource;
    private AudioSourceController _trackSource;

    private void Awake()
    {
        Instance.GetComponent<Tank_Damage>().AddOnDeathAction(StopEngineSound);
    }

    public void MoveSoundUpdate(float speedPercent)
    {
        if(speedPercent >= 0.25f)
        {
            _trackSource?.SetVolume(0.3f);
            _trackSource?.SetPitch(speedPercent);
            speedPercent = 0.6f * speedPercent;
            _engineSource?.SetVolume(speedPercent);
        }
        else
        {
            _trackSource?.SetVolume(0f);
            _engineSource?.SetVolume(0.2f);
        }
    }

    public void StartEngineSound()
    {
        _engineSource = PlaySound(SoundType.Engine, AudioMixerType.Sfx, 0.2f, true);
        _engineSource.transform.SetParent(Instance.transform);
        _trackSource = PlaySound(SoundType.Track, AudioMixerType.Sfx, 0f, true);
        _trackSource.transform.SetParent(Instance.transform);
    }

    public void StopEngineSound()
    {
        _engineSource?.StopAudio();
        _engineSource = null;

        _trackSource?.StopAudio();
        _trackSource = null;
    }

    public AudioSourceController PlaySound(SoundType soundType, AudioMixerType type = AudioMixerType.Master, float volume = 1f, bool isLoop = false)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Instance.transform.position, Quaternion.identity);
        audioSource.SetSound((Instance as Tank).TankSound.GetAudioClip(soundType));
        audioSource.SetGroup(type);
        audioSource.SetVolume(volume);
        if (isLoop) audioSource.SetLoop();
        audioSource.Play();

        return audioSource;
    }
}
