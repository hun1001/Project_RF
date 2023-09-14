using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonoTypeSoundBoxSO", menuName = "SO/MonoTypeSoundBoxSO", order = 1)]
public class MonoTypeSoundBoxSO : ScriptableObject
{
    public AudioClip[] sounds;
    public AudioClip GetRandomAudioClip() => sounds[Random.Range(0, sounds.Length)];
}
