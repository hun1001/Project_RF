using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using Util;

public class MouseManager : MonoSingleton<MouseManager>
{
    private float _mouseScrollSensitive = 5f;
    public void SetMouseScrollSensitive(float mouseScrollSensitive)
    {
        _mouseScrollSensitive = mouseScrollSensitive;
        _mouseScrollSensitive = Mathf.Clamp(_mouseScrollSensitive, 1f, 10f);
    }

    public Vector2 MouseDir = Vector2.zero;
    public float MouseMagnitude = 0f;

    private Vector2 _mousePosition = Vector2.zero;

    public Action OnMouseLeftButtonDown = null;
    public Action OnMouseLeftButtonUp = null;

    public Action OnMouseRightButtonDown = null;
    public Action OnMouseRightButtonUp = null;

    private Player _player;
    private bool _isPlayerDead = false;

    private void Start()
    {
        OnSceneLoaded();
        SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        _player = FindObjectOfType<Player>();
        _player?.Tank.GetComponent<Tank_Damage>().AddOnDeathAction(() => _isPlayerDead = true);
    }

    private void Update()
    {
        if (!(SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene))
        {
            return;
        }

        Vector2 basePosition = _player.Tank.transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            _mousePosition = hit.point;
        }

        Vector2 mouseDir = (_mousePosition - basePosition);

        MouseDir = mouseDir.normalized;
        MouseMagnitude = mouseDir.magnitude;

        Camera.main.GetComponent<CameraManager>().CameraZoom(-Input.mouseScrollDelta.y * _mouseScrollSensitive);

        if (Input.GetMouseButton(1) && _isPlayerDead == false)
        {
            transform.position = _mousePosition + mouseDir * 10;
        }
        else
        {
            transform.position = _player.Tank.transform.position;
        }

        if(Input.GetMouseButtonDown(0))
        {
            OnMouseLeftButtonDown?.Invoke();
        }

        if(Input.GetMouseButtonDown(1))
        {
            OnMouseRightButtonDown?.Invoke();
        }

        if(Input.GetMouseButtonUp(0))
        {
            OnMouseLeftButtonUp?.Invoke();
        }

        if(Input.GetMouseButtonUp(1))
        {
            OnMouseRightButtonUp?.Invoke();
        }
    }

    public void ClearMouseButtonAction()
    {
        OnMouseLeftButtonDown = null;
        OnMouseLeftButtonUp = null;
        OnMouseRightButtonDown = null;
        OnMouseRightButtonUp = null;
    }
}
