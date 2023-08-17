using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Mouse : MonoSingleton<Mouse>
{
    private void Update()
    {
        transform.position = Input.mousePosition;
        Debug.Log(transform.position);
    }
}
