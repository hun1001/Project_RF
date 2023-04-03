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
    
    public void Play()
    {
        _audioSource.Play();
        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        PoolManager.Pool("AudioSource", gameObject);
    }

    public void PoolObjectReset()
    {
        _audioSource.clip = null;
    }
}
