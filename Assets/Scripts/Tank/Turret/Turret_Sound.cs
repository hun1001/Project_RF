using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Turret_Sound : Turret_Component
{
    public void PlaySound(SoundType soundType)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", (Instance as Turret).FirePoint.position, (Instance as Turret).FirePoint.rotation);
        audioSource.SetSound((Instance as Turret).TurretSound.GetAudioClip(soundType));
        audioSource.Play();
    }
}
