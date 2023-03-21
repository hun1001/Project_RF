using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Move : Shell_Component
{
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * (Instance as Shell).Speed);
    }
}
