using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Turret_AimLine : Turret_Component
{
    private LineRenderer _lineRenderer = null;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // TODO : 사용중인 총알 가져와서 거리 계산하기 속도 * 2f
        _lineRenderer.SetPosition(0, Turret.FirePoint.position);
        _lineRenderer.SetPosition(1, Turret.FirePoint.position + Turret.FirePoint.up * Turret.CurrentShell.Speed * 2f);
    }
}
