using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class LaserBeam : CustomObject
{
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    public void SetLaserBeam(Vector3 startPosition, Vector3 endPosition)
    {
        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, endPosition);

        StartCoroutine(LaserBeamCoroutine());
    }

    private IEnumerator LaserBeamCoroutine()
    {
        yield return null;
        // 대충 실행끝나면 알아서 풀
    }
}
