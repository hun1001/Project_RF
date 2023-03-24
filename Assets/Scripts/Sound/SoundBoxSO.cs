using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "SoundBoxSO", menuName = "SO/SoundBoxSO", order = 1)]
public class SoundBoxSO : ScriptableObject
{
    public List<SoundData> SoundDataset;

    public AudioClip GetAudioClip(SoundType soundType) => SoundDataset.FirstOrDefault(x => x.SoundType == soundType).AudioClip;
}
