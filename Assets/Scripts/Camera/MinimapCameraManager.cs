using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;
using Util;

public class MinimapCameraManager : MonoBehaviour
{
    private Camera _cam;
    private Vector2 _size;
    private float _reciprocal;
    private float _rotation;
    private Vector2 _distance = Vector2.zero;
    private Vector3 _position = Vector3.zero;


    private void Start()
    {
        TryGetComponent(out _cam);
        _size = new Vector2(_cam.orthographicSize - 7.5f, _cam.orthographicSize * _cam.aspect - 7.5f);
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void ShowBorderIndicator(Transform target, Transform indicator)
    {
        _position = target.position;
        _distance = new Vector2(transform.position.x - _position.x, transform.position.y - _position.y);

        _distance = Quaternion.Euler(0, 0, target.eulerAngles.y) * _distance;

        // X axis
        if (Mathf.Abs(_distance.x) > Mathf.Abs(_distance.y))
        {
            _reciprocal = Mathf.Abs(_size.x / _distance.x);
            //_rotation = (_distance.x > 0) ? -90 : 90;
        }
        // Y axis
        else
        {
            _reciprocal = Mathf.Abs(_size.y / _distance.y);
            //_rotation = (_distance.y > 0) ? 0 : 180;
        }
        _rotation = Mathf.Atan2(transform.position.y - _position.y, transform.position.x - _position.x) * Mathf.Rad2Deg - 90f;

        indicator.localPosition = new Vector3(_distance.x * -_reciprocal, _distance.y * -_reciprocal, 1);
        indicator.localEulerAngles = new Vector3(0, 0, _rotation);
    }
}
