using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Missile : MonoBehaviour
{
    private Vector3 _targetPosition = Vector3.zero;
    private float _duration = 1f;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        SetMissile(new Vector3(10, 0, 0));
    }

    private void SetMissile(Vector3 targetPosition, bool curveRight = true)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPosition;

        Vector3 midPos = (startPos + endPos) / 2f + Vector3.back * 5f;

        float distance = Vector3.Distance(startPos, endPos);

        float height = endPos.y - startPos.y;

        transform.DOPath(new[] { startPos, midPos, endPos }, _duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("도착!");
            });
    }
}
