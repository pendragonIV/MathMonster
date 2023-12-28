using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Transform bg;

    private void Start()
    {

        SetupGameLogo();
        SetupPlayButton();
        bg.DOShakePosition(10f, 100f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1);

    }

    private void SetupPlayButton()
    {
        playButton.interactable = false;
        playButton.GetComponent<CanvasGroup>().alpha = 0f;
        playButton.GetComponent<RectTransform>().localScale = Vector3.one * 15f;

        playButton.GetComponent<CanvasGroup>().DOFade(1, 1f).SetUpdate(true);
        playButton.GetComponent<RectTransform>().DOScale(1, 1f).SetEase(Ease.OutElastic).SetUpdate(true).OnComplete(() =>
        {
            playButton.interactable = true;
        });
    }

    private void SetupGameLogo()
    {
        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, .5f).SetUpdate(true);
        gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 30);
        gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(432, 30), 1.5f).SetEase(Ease.OutElastic).SetUpdate(true).OnComplete(() =>
        {
            gameLogo.DORotate(Vector3.zero, .3f).SetEase(Ease.OutElastic).SetUpdate(true);
        });
    }
}
