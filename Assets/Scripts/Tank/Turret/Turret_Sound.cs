using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Turret_Sound : Turret_Component
{
    public void PlaySound(SoundType soundType, SoundGroupType groupType = SoundGroupType.Master)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Turret.FirePoint.position, Turret.FirePoint.rotation);
        audioSource.SetSound(Turret.TurretSound.GetAudioClip(soundType));
        audioSource.Play();
    }
}
