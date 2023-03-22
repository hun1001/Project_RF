using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SSSSS : MonoBehaviour
{
    public void ChangeScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
}
