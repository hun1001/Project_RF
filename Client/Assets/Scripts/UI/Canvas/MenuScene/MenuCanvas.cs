using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : BaseCanvas
{
    public void OnStartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
        Pool.PoolManager.DeleteAllPool();
    }

    public void OnServerGameStart()
    {
        OnStartButton();
        ServerManager.Instance.ConnectToServer();
    }

    public void OnModeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Mode);
    }

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting);
    }

    public void OnShopButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Shop);
    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree);
    }
}
