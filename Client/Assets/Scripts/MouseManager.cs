using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class MouseManager : MonoSingleton<MouseManager>
{
    public Vector2 MouseDir = Vector2.zero;

    private void Update()
    {
        Vector2 centorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = Input.mousePosition;

        Vector2 mouseDir = (mousePosition - centorPosition).normalized;

        MouseDir = mouseDir.normalized;
    }
}
