using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class MouseManager : MonoSingleton<MouseManager>
{
    public Vector2 MouseDir = Vector2.zero;
    public float MouseMagnitude = 0f;

    public Action OnMouseLeftButtonDown = null;
    public Action OnMouseLeftButtonUp = null;

    public Action OnMouseRightButtonDown = null;
    public Action OnMouseRightButtonUp = null;

    private void Update()
    {
        Vector2 centorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = Input.mousePosition;

        Vector2 mouseDir = (mousePosition - centorPosition);

        MouseDir = mouseDir.normalized;
        MouseMagnitude = mouseDir.magnitude;


        if(Input.GetMouseButton(1))
        {
            transform.position = MouseDir * MouseMagnitude;
            transform.position += FindObjectOfType<Player>().transform.position;
        }
        else
        {
            transform.position = FindObjectOfType<Player>().transform.position;
        }

        if(Input.GetMouseButtonDown(0))
        {
            OnMouseLeftButtonDown?.Invoke();
        }

        if(Input.GetMouseButtonDown(1))
        {
            OnMouseRightButtonDown?.Invoke();
        }

        if(Input.GetMouseButtonUp(0))
        {
            OnMouseLeftButtonUp?.Invoke();
        }

        if(Input.GetMouseButtonUp(1))
        {
            OnMouseRightButtonUp?.Invoke();
        }
    }
}
