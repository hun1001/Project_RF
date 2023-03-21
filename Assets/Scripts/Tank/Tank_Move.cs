using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Move : Tank_Component
{
    private float _speed = 0f;

    private void Awake()
    {
        _speed = Tank.TankStatSO.Speed;
    }

    public void Move(float magnitude)
    {
        if (magnitude > 0)
        {
            transform.Translate(Vector3.up * magnitude * _speed * Time.deltaTime);
        }
    }
}
