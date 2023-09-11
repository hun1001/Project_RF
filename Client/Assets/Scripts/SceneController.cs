using Event;
using Pool;
using static UnityEngine.SceneManagement.SceneManager;

public static class SceneController 
{
    private const string _loadingSceneName = "LoadingScene";

    public static void ChangeScene(string sceneName)
    {
        PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
        MouseManager.Instance.ClearMouseButtonAction();

        LoadingSceneManager.NextScene(sceneName);
        LoadScene(_loadingSceneName);
    }
}
