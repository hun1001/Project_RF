using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _textImage = null;

    [SerializeField]
    private GameObject _loadingLogo = null;
    
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            _textImage.SetActive(false);
            _loadingLogo.SetActive(true);

            StartCoroutine(LoadMenuSceneCoroutine());
        }
    }

    private IEnumerator LoadMenuSceneCoroutine()
    {
        var op = SceneManager.LoadSceneAsync("MenuScene");
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while(!op.isDone&&timer<=3)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        op.allowSceneActivation = true;
    }
}
