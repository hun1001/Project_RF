using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Move : Shell_Component
{
    private Rigidbody2D _rigidbody = null;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody.velocity = transform.up * (Instance as Shell).Speed;
    }
}
