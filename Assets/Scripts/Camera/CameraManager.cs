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

    }

    private IEnumerator CameraShakeCoroutine(float a, float f, float d)
    {

        yield return new WaitForSeconds(d);
    }
}
