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
        Vector2 centorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            Debug.Log(hit.point);
        }

        Vector2 mouseDir = (mousePosition - centorPosition);

        MouseDir = mouseDir.normalized;
        MouseMagnitude = mouseDir.magnitude;

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            Camera.main.GetComponent<CameraManager>().CameraZoom(-Input.mouseScrollDelta.y * _mouseScrollSensitive);

            if (Input.GetMouseButton(1) && _isPlayerDead == false)
            {
                transform.position = MouseDir * MouseMagnitude;
                transform.position += _player.transform.position;
            }
            else
            {
                transform.position = _player.transform.position;
            }
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
