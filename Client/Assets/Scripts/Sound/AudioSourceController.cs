using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class AudioSourceController : MonoBehaviour, IPoolReset
{
    [SerializeField]
    private AudioSource _audioSource = null;

    public void SetSound(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
    }

    public void SetGroup(AudioMixerType type = AudioMixerType.Master)
    {
        _audioSource.outputAudioMixerGroup = SoundManager.Instance.GetAudioMixerGroup(type);
    }

    public void SetLoop()
    {
        _audioSource.loop = true;
    }

    public void SetVolume(float volume = 1f)
    {
        _audioSource.volume = volume;
    }

    public void SetDimensionSound(float dimension)
    {
        _audioSource.spatialBlend = dimension;
    }

    public void SetPitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }

    public void Play()
    {
        _audioSource.Play();
        if (_audioSource.loop == false || gameObject.activeSelf == false)
            StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        _audioSource.Stop();
        PoolManager.Pool("AudioSource", gameObject);
    }

    public void Stop() => _audioSource.Stop();

    public void StopAudio()
    {
        _audioSource.Stop();

        PoolManager.Pool("AudioSource", gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void PoolObjectReset()
    {
        _audioSource.clip = null;
        _audioSource.loop = false;
        _audioSource.spatialBlend = 1f;
    }
}
