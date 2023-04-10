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
        ServerManager.Instance.ConnectToServer();
        Pool.PoolManager.DeleteAllPool();
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

    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree);
    }

    public void OnFreeGoodsButton()
    {

    }

    public void OnPaidGoodsButton()
    {

    }

    public void OnFreeExperienceButton()
    {

    }
}
