using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Event;
using UnityEngine.SceneManagement;

public class ModeCanvas : BaseCanvas
{
    [Header("Modes")]
    [SerializeField]
    private RectTransform _modesFrame;
    private Image[] _modeImages;

    [Header("Maps")]
    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private RectTransform _scrollView;

    [Header("Warning")]
    [SerializeField]
    private RectTransform _warningPanel;
    private Sequence _warningSequence;

    private Image[] _mapImages;

    private void Awake()
    {
        _mapImages = _content.GetComponentsInChildren<Image>();
        _modeImages = _modesFrame.GetComponentsInChildren<Image>();
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();

        _startSequence = DOTween.Sequence()
        .PrependCallback(() =>
        {
            foreach (Image image in _modeImages)
            {
                image.DOFade(0f, 0f);
            }
        })
        .Prepend(_modesFrame.DOAnchorPosX(-200f, 0f))
        .Append(_modesFrame.DOAnchorPosX(48f, 0.5f))
        .InsertCallback(0.25f, () =>
        {
            foreach (Image image in _modeImages)
            {
                image.DOFade(1f, 0.5f);
            }
        })
        .PrependCallback(() =>
        {
            _scrollView.anchoredPosition += Vector2.right * 600f;
            foreach (Image image in _mapImages)
            {
                image.DOFade(0.3f, 0f);
            }
        })
        .Append(_scrollView.DOAnchorPosX(0f, 1.2f))
        .InsertCallback(0.7f, () =>
        {
            foreach (Image image in _mapImages)
            {
                image.DOFade(1f, 1f);
            }
        });
    }

    private bool ShellEmptyCheck()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());

        return shellEquipmentData._shellEquipmentList[0] == "" && shellEquipmentData._shellEquipmentList[1] == "";
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

    public void OnBossMode()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }

    public void OnStageMode()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("StageTestScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }

    public void OnTrainingRoom()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("TrainingScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }
}
