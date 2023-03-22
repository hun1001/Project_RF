using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource = null;

    public void SetSound(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
    }

    public void Play()
    {
        _audioSource.Play();
    }
}
