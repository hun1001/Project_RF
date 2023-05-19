using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera = null;
    public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

    private Transform _parent;

    private void Awake()
    {
        _virtualCamera = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();

        _parent = transform.parent;
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
        _virtualCamera.transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _parent.rotation = Quaternion.identity;
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
}
