using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : MonoSingleton<GameManager>
{
    private void Start()
    {
        SceneManager.sceneUnloaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene)
    {
        KeyboardManager.Instance.ClearKeyActions();
    }
}
