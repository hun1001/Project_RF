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

    [SerializeField]
    private AudioSource _audioSource = null;

    private CustomObject _owner = null;
    private Vector3 _startPosition = Vector3.zero;

    private CameraManager _cameraManager = null;

    public void SetLaserBeam(CustomObject owner, Vector3 startPosition, Vector3 endPosition)
    {
        _owner = owner;

        _meshRenderer.enabled = false;
        _collider.enabled = false;

        _cameraManager ??= Camera.main.GetComponent<CameraManager>();

        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, endPosition);

        transform.localScale = new Vector3(1, Vector3.Distance(startPosition, endPosition) / 2, 1);
        transform.position = (startPosition + endPosition) / 2;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg + 90);

        _startPosition = startPosition;

        StartCoroutine(LaserBeamCoroutine());
    }

    private IEnumerator LaserBeamCoroutine()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(1f);

        _meshRenderer.enabled = true;
        _collider.enabled = true;

        _cameraManager.CameraShake(3, 5, 0.5f);

        yield return new WaitForSeconds(1f);

        _collider.enabled = false;

        transform.DOScaleX(0, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => PoolManager.Pool(ID, gameObject));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<CustomObject>() != _owner)
        {
            other.GetComponent<Tank_Damage>().Damaged(1, 99999, other.ClosestPoint(transform.position), _startPosition);
        }
    }
}
