using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class MenuSceneCameraManager : MonoBehaviour
{
    [SerializeField]
    private MenuCanvas _menuCanvas = null;
    private CinemachineFreeLook _cam;

    private bool _isOnUI;

    private void Awake()
    {
        TryGetComponent(out _cam);
    }
    void Update()
    {
#if UNITY_EDITOR
        _isOnUI = EventSystem.current.IsPointerOverGameObject();
#else
        if(Input.touchCount > 0)
        {
            _isOnUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
#endif

        if (Input.GetMouseButtonDown(0) && _isOnUI == false)
        {
            _cam.m_XAxis.m_InputAxisName = "Mouse X";
            _cam.m_YAxis.m_InputAxisName = "Mouse Y";
            _cam.m_YAxis.m_InvertInput = true;

            _menuCanvas.CameraUIHide(false);
        }
        if (Input.GetMouseButtonUp(0) && _isOnUI == false)
        {
            _cam.m_XAxis.m_InputAxisName = "";
            _cam.m_YAxis.m_InputAxisName = "";
            _cam.m_YAxis.m_InvertInput = false;

            _cam.m_XAxis.m_InputAxisValue = 0f;
            _cam.m_YAxis.m_InputAxisValue = 0f;

            _menuCanvas.CameraUIHide(true);
        }

#if UNITY_EDITOR
        if (Input.mouseScrollDelta.y > 0 && _isOnUI == false)
        {
            _cam.m_Lens.FieldOfView -= 1f;
            _cam.m_Lens.FieldOfView = Mathf.Clamp(_cam.m_Lens.FieldOfView, 20f, 60f);
        }
        if (Input.mouseScrollDelta.y < 0 && _isOnUI == false)
        {
            _cam.m_Lens.FieldOfView += 1f;
            _cam.m_Lens.FieldOfView = Mathf.Clamp(_cam.m_Lens.FieldOfView, 20f, 60f);
        }
#else
        if (Input.touchCount == 2 && _isOnUI == false)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = prevMagnitude - currentMagnitude;

            Camera.main.fieldOfView += difference * 0.1f;

            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 60f);
        }
#endif
    }
}
