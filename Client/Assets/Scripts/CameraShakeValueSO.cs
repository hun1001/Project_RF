using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CameraShakeValueSO", menuName = "ScriptableObjects/CameraShakeValueSO", order = 1)]
public class CameraShakeValueSO : ScriptableObject
{
    [SerializeField]
    private float amplitudeGain = 0f;
    public float AmplitudeGain => amplitudeGain;

    [SerializeField]
    private float frequencyGain = 0f;
    public float FrequencyGain => frequencyGain;

    [SerializeField]
    private float duration = 0f;
    public float Duration => duration;
}
