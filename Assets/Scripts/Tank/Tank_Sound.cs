using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Sound : Tank_Component
{
    public void MoveSoundUpdate(float speedPercent)
    {

    }

    public void StartEngineSound()
    {

    }

    public void PlaySound(SoundType soundType, AudioMixerType type = AudioMixerType.Master, bool isLoop = false)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Instance.transform.position, Quaternion.identity, Instance.transform);
        audioSource.SetSound((Instance as Tank).TankSound.GetAudioClip(soundType));
        audioSource.SetGroup(type);
        if (isLoop) audioSource.SetLoop();
        audioSource.Play();
    }
}
