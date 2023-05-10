using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : BaseCanvas
{
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

    [Header("Buttons")]
    [SerializeField]
    private Button _startButton = null;

    [SerializeField]
    private Button _trainingButton = null;

    [SerializeField]
    private Button _serverButton = null;

    [Header("Warning")]
    [SerializeField]
    private RectTransform _warningPanel;
    [SerializeField]
    private TextController _warningText;
    private Sequence _warningSequence;

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

        _startButton.interactable = true;
        _trainingButton.interactable = true;

        _startButton.onClick.AddListener(OnStartButton);
        _trainingButton.onClick.AddListener(OnTrainingStart);
        _serverButton.onClick.AddListener(OnServerButton);

        _warningPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        _isHide = true;
        _isOpen = true;
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        _isHide = true;

        _startSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(32f, 0.25f);
                _bottomFrame.DOAnchorPosY(-55f, 0.25f);
                _leftFrame.DOAnchorPosX(-52f, 0.25f);
                if (_isCameraMove == false)
                {
                    _showButton.DOAnchorPosY(-_showButton.sizeDelta.y, 0.25f);
                }
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                _bottomFrame.DOAnchorPosY(0f, 0.25f);
                _leftFrame.DOAnchorPosX(0f, 0.25f);
                _showButton.DOAnchorPosY(0f, 0.25f);
            }
        });
    }

    public void OnStartButton()
    {
        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _startButton.interactable = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
        Pool.PoolManager.DeleteAllPool();
    }

    public void OnTrainingStart()
    {
        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _trainingButton.interactable = false;

        Time.timeScale = 1;
        SceneManager.LoadScene("TrainingScene");
        Pool.PoolManager.DeleteAllPool();
    }

    public void OnServerButton()
    {
        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _serverButton.interactable = false;
        ServerManager.Instance.ConnectToServer();
    }

    private bool ShellEmptyCheck()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());

        return shellEquipmentData._shellEquipmentData.Count <= 0;
    }

    private void WarningShellEmpty()
    {
        _warningSequence.Kill();
        _warningSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
            _warningPanel.gameObject.SetActive(true);
            _warningText.SetText("총알이 장착되어 있지 않습니다!\n총알을 장착해주세요.");
        })
        .AppendInterval(1.2f)
        .Append(_warningPanel.GetComponent<CanvasGroup>().DOFade(0, 1f))
        .AppendCallback(() =>
        {
            _warningPanel.gameObject.SetActive(false);
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
        });
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

    public void OnOpenItem()
    {
        CanvasManager.ChangeCanvas(CanvasType.Gear, CanvasType);
    }

    public void UIHide(bool isHide)
    {
        _isCameraMove = false;
        _isHide = isHide;

        _startSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(32f, 0.25f);
                _bottomFrame.DOAnchorPosY(-55f, 0.25f);
                _leftFrame.DOAnchorPosX(-52f, 0.25f);
                if (_isCameraMove == false)
                {
                    _showButton.DOAnchorPosY(-_showButton.sizeDelta.y, 0.25f);
                }
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                _bottomFrame.DOAnchorPosY(0f, 0.25f);
                _leftFrame.DOAnchorPosX(0f, 0.25f);
                _showButton.DOAnchorPosY(0f, 0.25f);
            }
        });
    }

    public void CameraUIHide(bool isHide)
    {
        _isCameraMove = true;
        _isHide = isHide;

        _startSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(32f, 0.25f);
                _bottomFrame.DOAnchorPosY(-55f, 0.25f);
                _leftFrame.DOAnchorPosX(-52f, 0.25f);
                if (_isCameraMove == false)
                {
                    _showButton.DOAnchorPosY(-_showButton.sizeDelta.y, 0.25f);
                }
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                _bottomFrame.DOAnchorPosY(0f, 0.25f);
                _leftFrame.DOAnchorPosX(0f, 0.25f);
                _showButton.DOAnchorPosY(0f, 0.25f);
            }
        });
    }
}
