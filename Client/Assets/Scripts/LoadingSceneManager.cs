using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    private static string _nextSceneName = string.Empty;

    public static void NextScene(string nextScene)
    {
        _nextSceneName = nextScene;
    }

    private void Start()
    {
        StartCoroutine(LoadSceneCoroutine(_nextSceneName));
    }

    private IEnumerator LoadSceneCoroutine(string nextScene)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
            }
            else
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }

}
