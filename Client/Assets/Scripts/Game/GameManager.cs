using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : MonoSingleton<GameManager>
{
    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void Update()
    {
        _input.OnUpdate();
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _input.KeyAction = null;
    }
}
