using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera = null;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetPlayer(Transform target)
    {
        _virtualCamera.Follow = target;
        _virtualCamera.LookAt = target;
    }
}
