using Addressable;
using DG.Tweening;
using Event;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : BaseCanvas
{
    [Header("Goods")]
    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    [Header("CurrentTankInfo")]
    [SerializeField]
    private MenuTankInfoUI _menuTankInfoUI = null;

    [Header("Shell")]
    [SerializeField]
    private Image[] _shellImages = null;
    private Sprite _plusSprite = null;
    [SerializeField]
    private GameObject _shellReplacement = null;
    private bool _isShellOpen = false;

    private string _currentTankID;
    private ShellEquipmentData _shellEquipmentDataDict;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _topFrame = null;
    [SerializeField]
    private RectTransform _bottomFrame = null;

    private bool _isHide = false;

    [Header("Hanger")]
    [SerializeField]
    private HangerHandle _hangerHandle = null;

    [Header("Buttons")]
    [SerializeField]
    private Button _startButton = null;

    [Header("Other")]
    [SerializeField]
    private RectTransform _warningPanel;
    private Sequence _warningSequence;
    [SerializeField]
    private GameObject _keyGuidePanel;
    private bool _isGuideOpen = false;

    private void Awake()
    {
        GoodsManager.IncreaseFreeGoods(0);
        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);

        _startButton.interactable = true;
        _startButton.onClick.AddListener(OnStartButton);

        _plusSprite = AddressablesManager.Instance.GetResource<Sprite>("PlusImage");

        _warningPanel.gameObject.SetActive(false);

        EventManager.StartListening(EventKeyword.MenuCameraMove, CameraUIHide);

        _hangerHandle.Init();

        _hangerHandle.FilterInit();
    }

    private void Start()
    {
        _isOpen = true;
        _isHide = false;
        _isShellOpen = false;

        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();

        EventManager.StartListening(EventKeyword.ShellReplacement, ShellCheck);

        ShellCheck();
        _hangerHandle.HangerUpdate();
        CurrentTankInfoUpdate();
        _hangerHandle.HangerSort();
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        _isShellOpen = false;
        _isHide = false;
        
        _shellReplacement.SetActive(false);

        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();

        _hangerHandle.OpenEvent();

        ShellCheck();
        _hangerHandle.HangerUpdate();
        CurrentTankInfoUpdate();
        _hangerHandle.HangerSort();
    }

    public void CurrentTankInfoUpdate()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();

        _menuTankInfoUI.CurrentTankInfoUpdate();

        _hangerHandle.CurrentTankInfoUpdate();
    }

    #region Shell
    private void ShellCheck()
    {
        int idx = 0;
        _shellEquipmentDataDict = ShellSaveManager.GetShellEquipment(_currentTankID);

        foreach (var shell in _shellEquipmentDataDict._shellEquipmentList)
        {
            if (shell == "")
            {
                _shellImages[idx++].sprite = _plusSprite;
                continue;
            }

            Shell shellData = AddressablesManager.Instance.GetResource<GameObject>(shell).GetComponent<Shell>();
            _shellImages[idx].sprite = shellData.ShellSprite;
            _shellImages[idx++].gameObject.SetActive(true);
        }
    }

    private bool ShellEmptyCheck()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());

        return shellEquipmentData._shellEquipmentList[0] == "" && shellEquipmentData._shellEquipmentList[1] == "" && shellEquipmentData._shellEquipmentList[2] == "";
    }

    private void WarningShellEmpty()
    {
        _warningSequence.Kill();
        _warningSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
            _warningPanel.gameObject.SetActive(true);
            _warningPanel.GetChild(0).GetComponent<TextController>().SetText("No bullets loaded!\nEquip the bullet.");
        })
        .AppendInterval(1.2f)
        .Append(_warningPanel.GetComponent<CanvasGroup>().DOFade(0, 1f))
        .AppendCallback(() =>
        {
            _warningPanel.gameObject.SetActive(false);
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
        });
    }
    #endregion

    #region Hanger
    public void OpenHanger()
    {
        PlayButtonSound();

        _hangerHandle.OpenHanger();
    }


    #endregion

    #region Buttons
    public void OnStartButton()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _startButton.interactable = false;
        Time.timeScale = 1;
        SceneController.ChangeScene("GameScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting, CanvasType);

        PlayButtonSound();
    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree, CanvasType);

        PlayButtonSound();
    }

    public void OnOpenShell()
    {
        PlayButtonSound();
        if (_isShellOpen)
        {
            _isShellOpen = false;
            _shellReplacement.SetActive(false);
        }
        else
        {
            _isShellOpen = true;
            _shellReplacement.SetActive(true);
        }
    }

    public void OnGuideOpen()
    {
        PlayButtonSound();
        if (_isGuideOpen)
        {
            _isGuideOpen = false;
            _keyGuidePanel.SetActive(false);
        }
        else
        {
            _isGuideOpen = true;
            _keyGuidePanel.SetActive(true);
        }
    }
    #endregion

    #region Animations
    public void CameraUIHide(object[] isHide)
    {
        _isHide = (bool)isHide[0];

        DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(60f, 0.25f);
                _bottomFrame.DOAnchorPosY(-330f, 0.25f);
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                _bottomFrame.DOAnchorPosY(0f, 0.25f);
            }
        });

        if (_hangerHandle.gameObject.activeSelf)
        {
            _hangerHandle.gameObject.SetActive(false);
        }
    }
    #endregion
}
