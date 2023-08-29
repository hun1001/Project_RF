using Event;
using UnityEngine;
using Util;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    // Tutorial ing
    private bool _isTutorial = false;
    public bool IsTutorial => _isTutorial;

    private void Awake()
    {
        //PlayerPrefs.SetInt("Tutorial", 0);
        //_isTutorial = true;
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            TutorialStart();
        }
    }

    public void TutorialStart()
    {
        _isTutorial = true;
        PlayerPrefs.SetInt("Tutorial", 1);

        FindObjectOfType<BaseSceneCanvasManager>().ChangeCanvas(CanvasType.Tutorial);
    }

    public void TutorialEnd()
    {
        _isTutorial = false;
    }

    public void TutorialSkip()
    {
        _isTutorial = false;
    }

    public void GameTutorialStart()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            WaveManager.Instance.gameObject.SetActive(false);
        }
    }

    public void TankDummySpawn()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            var tank = SpawnManager.Instance.SpawnUnit("BT-5", new Vector3(22f, 25f, 0f), Quaternion.identity, GroupType.Enemy);
            tank.GetComponent<Tank_Damage>().AddOnDeathAction(() => EventManager.TriggerEvent(EventKeyword.NextTutorial));
        }
    }

    public void MovingTargetSpawn()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            GameObject.Find("MoveTarget").transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void TutorialWaveStart()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            WaveManager.Instance.gameObject.SetActive(true);
        }
    }
}
