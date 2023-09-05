using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;

public static class SceneController 
{
    private const string _loadingSceneName = "LoadingScene";

    public static void ChangeScene(string sceneName)
    {
        LoadingSceneManager.NextScene(sceneName);
        LoadScene(_loadingSceneName);
    }
}
