using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using Event;

public class MenuSceneCameraManager : MonoBehaviour
{
    private CinemachineFreeLook _cam;

    private bool _isOnUI;
    private bool _isMouseClick = false;

    private void Awake()
    {
        TryGetComponent(out _cam);
    }

    void Update()
    {
        _isOnUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetMouseButtonDown(0) && _isOnUI == false)
        {
            _cam.m_XAxis.m_InputAxisName = "Mouse X";
            _cam.m_YAxis.m_InputAxisName = "Mouse Y";

            _cam.m_YAxis.m_InvertInput = true;
            _isMouseClick = true;

            EventManager.TriggerEvent(EventKeyword.MenuCameraMove, false);
        }
        if (Input.GetMouseButtonUp(0) && _isMouseClick)
        {
            _cam.m_XAxis.m_InputAxisName = "";
            _cam.m_YAxis.m_InputAxisName = "";

            _cam.m_YAxis.m_InvertInput = false;
            _isMouseClick = false;

            _cam.m_XAxis.m_InputAxisValue = 0f;
            _cam.m_YAxis.m_InputAxisValue = 0f;

            EventManager.TriggerEvent(EventKeyword.MenuCameraMove, true);
        }
        if (_isOnUI == false)
        {
            _cam.m_Lens.FieldOfView += -Input.GetAxis("Mouse ScrollWheel") * 10;
            _cam.m_Lens.FieldOfView = Mathf.Clamp(_cam.m_Lens.FieldOfView, 30, 75);
        }
    }
}
