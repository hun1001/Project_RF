using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : BaseCanvas
{
    [Header("Hanger")]
    // [SerializeField]
    // private RectTransform _smallHangerContent;
    // [SerializeField]
    // private GameObject _smallHangerTemplate;
    [SerializeField]
    private GameObject _bigHanger;
    [SerializeField]
    private RectTransform _bigHangerContent;
    [SerializeField]
    private GameObject _bigHangerTemplate;

    [Header("Goods")]
    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _topFrame;
    [SerializeField]
    private RectTransform _bottomFrame;
    [SerializeField]
    private RectTransform _leftFrame;
    [SerializeField]
    private RectTransform _showButton;

    private bool _isHide = false;
    private bool _isCameraMove = false;

    private void Awake()
    {
        //foreach (var tankInfo in 보유한탱크리스트)
        // for(int i = 0; i < 5; i++)
        // {
        //     var smallTank = Instantiate(_smallHangerTemplate, _smallHangerContent);
        //     smallTank.SetActive(true);

        //     var bigTank = Instantiate(_bigHangerTemplate, _bigHangerContent);
        //     bigTank.SetActive(true);

        //     smallTank.GetComponent<Button>().onClick.AddListener(() =>
        //     {
        //         OnOpenItem();
        //     });
        //     bigTank.GetComponent<Button>().onClick.AddListener(() =>
        //     {
        //         OnOpenItem();
        //     });
        // }

        GoodsManager.AddOnGoodsChanged((f, p) =>
        {
            _goodsTexts.SetGoodsTexts(f, p);
        });

        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);
    }

    private void Start()
    {
        _isHide = true;

        _startSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(32f, 0.25f);
                _bottomFrame.DOAnchorPosY(-55f, 0.25f);
                _leftFrame.DOAnchorPosX(-52f, 0.25f);
                if(_isCameraMove == false)
                {
                    _showButton.DOAnchorPosY(-_showButton.sizeDelta.y, 0.25f);
                }
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                _bottomFrame.DOAnchorPosY(0f, 0.25f);
                _leftFrame.DOAnchorPosX(0f, 0.25f);
                if(_isCameraMove == false)
                {
                    _showButton.DOAnchorPosY(0f, 0.25f);
                }
            }
        });
    }

    public override void OnOpenAnimation()
    {
        _isHide = true;
        base.OnOpenAnimation();
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

    public void OnOpenItem()
    {
        CanvasManager.ChangeCanvas(CanvasType.Gear, CanvasType);
    }

    public void UIHide(bool isHide)
    {
        _isCameraMove = false;
        _isHide = isHide;
        _startSequence.Restart();
    }

    public void CameraUIHide(bool isHide)
    {
        _isCameraMove = true;
        _isHide = isHide;
        _startSequence.Restart();
    }
}
