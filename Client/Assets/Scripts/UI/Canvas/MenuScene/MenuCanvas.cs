using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : BaseCanvas
{
    [Header("Hanger")]
    [SerializeField]
    private RectTransform _smallHangerContent;
    [SerializeField]
    private GameObject _smallHangerTemplate;
    [SerializeField]
    private GameObject _bigHanger;
    [SerializeField]
    private RectTransform _bigHangerContent;
    [SerializeField]
    private GameObject _bigHangerTemplate;

    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    private void Awake()
    {
        //foreach(var tankInfo in 보유한탱크리스트)
        //{
        //    var smallTank = Instantiate(_smallHangerTemplate, _smallHangerContent);
        //    var bigTank = Instantiate(_bigHangerTemplate, _bigHangerContent);
        //    smallTank.GetComponent<Button>().onClick.AddListener(() =>
        //    {

        //    });
        //    bigTank.GetComponent<Button>().onClick.AddListener(() =>
        //    {

        //    });
        //}

        GoodsManager.AddOnGoodsChanged((f, p) =>
        {
            _goodsTexts.SetGoodsTexts(f, p);
        });

        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);
    }

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
        _bigHanger.SetActive(true);
    }

    public void OnCloseHanger()
    {
        _bigHanger.SetActive(false);
    }
}
