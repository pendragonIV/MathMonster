using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Text result;
    [SerializeField]
    private Text timeLeft;
    [SerializeField]
    private Text timeRemain;
    [SerializeField]
    private Transform bg;


    private void Start()
    {
        Camera.main.transform.DOShakePosition(8f, .3f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1);
    }

    public void SetTime(float totalTime, float time)
    {
        int minute = (int)time / 60;
        int second = (int)time % 60;

        float remain = totalTime - time;
        int minuteRemain = (int)remain / 60;
        int secondRemain = (int)remain % 60;

        timeLeft.text = minute.ToString("00") + ":" + second.ToString("00");
        timeRemain.text = minuteRemain.ToString("00") + ":" + secondRemain.ToString("00");
    }

    public void SetResult(double result)
    {
        this.result.text = result.ToString();
    }

    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        bg.GetComponent<RectTransform>().DOShakePosition(10f, 100f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1).SetUpdate(true);
    }

    public void ShowLosePanel()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
        bg.GetComponent<RectTransform>().DOShakePosition(10f, 100f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1).SetUpdate(true);
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
