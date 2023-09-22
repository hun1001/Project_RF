using UnityEngine;
using Cinemachine;
using Util;

public class VirtualCameraManager : MonoSingleton<VirtualCameraManager>
{
    private Transform virtualCamera = null;

    private void Awake()
    {
        virtualCamera = transform.GetChild(0).GetComponent<Transform>();
        virtualCamera.gameObject.SetActive(false);
    }

    public void SwitchingCamera(bool active = true)
    {
        virtualCamera.gameObject.SetActive(active);
    }

    public void SetTargetCamera(GameObject target)
    {
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = target.transform;
    }
}
