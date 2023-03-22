using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera = null;

    private void Awake()
    {
        _virtualCamera = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
    }

    public void SetPlayer(Transform target)
    {
        _virtualCamera.Follow = target;
        _virtualCamera.LookAt = target;
    }

    public void CameraShake(float amplitudeGain, float frequencyGain, float duration)
    {
        StartCoroutine(CameraShakeCoroutine(amplitudeGain, frequencyGain, duration));
    }

    private IEnumerator CameraShakeCoroutine(float a, float f, float d)
    {
        var cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // 진폭
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = a;
        // 횟수
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = f;

        yield return new WaitForSeconds(d);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }

    public void CameraZoomInEffect(float zoom, float delay, float duration)
    {
        StartCoroutine(CameraZoomInEffectCoroutine(zoom, delay, duration));
    }

    private IEnumerator CameraZoomInEffectCoroutine(float zoom, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        var origin = _virtualCamera.m_Lens.FieldOfView;
        var target = origin + zoom;

        var timer = 0f;
        while (timer < duration)
        {
            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(origin, target, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < duration)
        {
            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(target, origin, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        _virtualCamera.m_Lens.FieldOfView = origin;
    }


}
