using Pool;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _bgmAudioClip = null;

    private void Start()
    {
        PlaySound(SoundType.BGM, AudioMixerType.Bgm, 1f, true);
    }

    private AudioSourceController PlaySound(SoundType soundType, AudioMixerType type = AudioMixerType.Master, float volume = 1f, bool isLoop = false)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", transform.position, Quaternion.identity, Camera.main.transform);
        audioSource.SetSound(_bgmAudioClip);
        audioSource.SetGroup(type);
        audioSource.SetVolume(volume);
        if (isLoop) audioSource.SetLoop();
        audioSource.Play();

        return audioSource;
    }
}
