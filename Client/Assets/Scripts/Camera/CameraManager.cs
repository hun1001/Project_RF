using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera = null;
    public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

    [SerializeField]
    private CinemachineTargetGroup _targetGroup = null;
    public CinemachineTargetGroup TargetGroup => _targetGroup;

    [SerializeField]
    private Transform _parent;

    [SerializeField]
    private Volume _volume;

    public void CameraShake(float amplitudeGain, float frequencyGain, float duration)
    {
        StartCoroutine(CameraShakeCoroutine(amplitudeGain, frequencyGain, duration));
    }

    public void CameraShake(CameraShakeValueSO cameraShakeValueSO)
    {
        StartCoroutine(CameraShakeCoroutine(cameraShakeValueSO.AmplitudeGain, cameraShakeValueSO.FrequencyGain, cameraShakeValueSO.Duration));
    }

    private IEnumerator CameraShakeCoroutine(float a, float f, float d)
    {
        var cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // 진폭
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = a;
        // ?�수
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = f;

        yield return new WaitForSeconds(d);

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
        _virtualCamera.transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _parent.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        var cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // 진폭
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        // ?�수
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
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

    private bool _isZooming = false;

    public void CameraZoom(float fov, float duration)
    {
        StartCoroutine(CameraZoomCoroutine(fov, duration));
    }

    private IEnumerator CameraZoomCoroutine(float offsetZ, float duration)
    {
        if (_isZooming)
        {
            yield break;
        }

        _isZooming = true;

        var transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        var origin = transposer.m_FollowOffset.z;
        var target = offsetZ;

        var timer = 0f;
        while (timer < duration)
        {
            transposer.m_FollowOffset.z = Mathf.Lerp(origin, target, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transposer.m_FollowOffset.z = target;

        _isZooming = false;
    }

    public int TargetGroupLength => _targetGroup.m_Targets.Length;

    public void AddTargetGroup(Transform target, float weight, float radius)
    {
        if (_targetGroup.FindMember(target) != -1)
        {
            return;
        }

        _targetGroup.AddMember(target, weight, radius);
    }


    public void SetVolumeVignette(Color color, float intensity, float smoothness, float duration)
    {
        StartCoroutine(VolumeVignetteCoroutine(color, intensity, smoothness, duration));
    }

    private IEnumerator VolumeVignetteCoroutine(Color color, float intensity, float smoothness, float duration)
    {
        Vignette vignette = null;
        if (_volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.color.value = color;
            vignette.intensity.value = intensity;
            vignette.smoothness.value = smoothness;
            yield return new WaitForSeconds(duration);
            vignette.color.value = Color.black;
            vignette.intensity.value = 0;
            vignette.smoothness.value = 0.2f;
        }
    }
}
