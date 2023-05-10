using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using DG.Tweening;

public class LaserBeam : CustomObject
{
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    [SerializeField]
    private MeshRenderer _meshRenderer = null;

    [SerializeField]
    private BoxCollider2D _collider = null;

    private void Start() => SetLaserBeam(Vector3.zero, new Vector3(0, 5, 0));

    public void SetLaserBeam(Vector3 startPosition, Vector3 endPosition)
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;

        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, endPosition);

        transform.localScale = new Vector3(1, Vector3.Distance(startPosition, endPosition) / 2, 1);
        transform.position = (startPosition + endPosition) / 2;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg + 90);

        StartCoroutine(LaserBeamCoroutine());
    }

    private IEnumerator LaserBeamCoroutine()
    {
        yield return new WaitForSeconds(1f);

        _meshRenderer.enabled = true;
        _collider.enabled = true;

        yield return new WaitForSeconds(1f);

        _collider.enabled = false;

        transform.DOScaleX(0, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => Destroy(gameObject));
        //PoolManager.Pool(ID, gameObject);
    }
}