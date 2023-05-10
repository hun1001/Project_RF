using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Missile : MonoBehaviour
{
    private Vector3 _targetPosition = Vector3.zero;

    private void Start() => SetMissile(new Vector3(0, 5, 0));
    private void SetMissile(Vector3 targetPosition, bool curveRight = true)
    {
        _targetPosition = targetPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOPath(new Vector3[] { transform.position, targetPosition }, 1f, PathType.CatmullRom, PathMode.Full3D, 10, Color.red).SetEase(Ease.InOutSine));
        // look move direction
        sequence.Join(transform.DORotate(new Vector3(0, 0, Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg + 90), 0.5f).SetEase(Ease.InOutSine));

        //sequence.OnComplete(() => Destroy(gameObject));

        sequence.Play();
    }
}
