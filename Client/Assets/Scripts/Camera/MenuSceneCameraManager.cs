using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class MenuSceneCameraManager : MonoBehaviour
{
    private CinemachineFreeLook _cam;

    private void Awake()
    {
        TryGetComponent(out _cam);
    }
    void Update()
    {
        bool isOnUI;
#if UNITY_EDITOR
        isOnUI = EventSystem.current.IsPointerOverGameObject();
#else
        isOnUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif

        if (Input.GetMouseButtonDown(0) && isOnUI == false)
        {
            _cam.m_XAxis.m_InputAxisName = "Mouse X";
            _cam.m_YAxis.m_InputAxisName = "Mouse Y";
            _cam.m_YAxis.m_InvertInput = true;

        }
        if (Input.GetMouseButtonUp(0))
        {
            _cam.m_XAxis.m_InputAxisName = "";
            _cam.m_YAxis.m_InputAxisName = "";
            _cam.m_YAxis.m_InvertInput = false;

            _cam.m_XAxis.m_InputAxisValue = 0f;
            _cam.m_YAxis.m_InputAxisValue = 0f;
        }
    }
}