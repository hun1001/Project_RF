using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using Event;

public class MenuSceneCameraManager : MonoBehaviour
{
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

            EventManager.TriggerEvent(EventKeyword.CameraMove, false);
        }
        if (Input.GetMouseButtonUp(0) && _isOnUI == false)
        {
            _cam.m_XAxis.m_InputAxisName = "";
            _cam.m_YAxis.m_InputAxisName = "";

            _cam.m_YAxis.m_InvertInput = false;

            _cam.m_XAxis.m_InputAxisValue = 0f;
            _cam.m_YAxis.m_InputAxisValue = 0f;

            EventManager.TriggerEvent(EventKeyword.CameraMove, true);
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            _cam.m_Lens.FieldOfView += deltaMagnitudeDiff * 3 * Time.deltaTime;
            _cam.m_Lens.FieldOfView = Mathf.Clamp(_cam.m_Lens.FieldOfView, 15, 40);
        }

    }
}
