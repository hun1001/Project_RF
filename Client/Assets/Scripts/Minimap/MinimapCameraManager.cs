using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraManager : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, -20);

    private Transform _target = null;

    public Transform Target
    {
        set => _target = value;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position + offset;
        }
    }
}
