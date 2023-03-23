using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraManager : MonoBehaviour
{
    private Camera _cam;
    private Vector2 _size;

    private void Start()
    {
        TryGetComponent(out _cam);
        _size = new Vector2(_cam.orthographicSize, _cam.orthographicSize * _cam.aspect);
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
