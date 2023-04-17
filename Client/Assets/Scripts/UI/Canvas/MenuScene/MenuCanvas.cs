using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : BaseCanvas
{
    [Header("Hanger")]
    [SerializeField]
    private GameObject _hanger;

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
        CanvasManager.ChangeCanvas(CanvasType.Mode, CanvasType);
    }

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting, CanvasType);
    }

    public void OnShopButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Shop, CanvasType);
    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree, CanvasType);
    }

    public void OnOpenHanger()
    {
        _hanger.SetActive(true);
    }

    public void OnCloseHanger()
    {
        _hanger.SetActive(false);
    }
}
