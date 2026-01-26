using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FightUI : MonoBehaviour
{
    [SerializeField] CanvasGroup hitPanel;
    [SerializeField] PlayerTurnController turnController;
    [SerializeField] Scrollbar bar;
    [SerializeField] RectTransform hitImage;
    [SerializeField] RectTransform meterRect; // <- das RectTransform vom Bar-Background/Container
    Color originalNormalColor;

    [SerializeField] CanvasGroup WinLosePanel;
    [SerializeField] TextMeshProUGUI WinLose;
    void OnEnable()
    {
        TrySetupHitZone();
    }

    
    void TrySetupHitZone()
    {
        if (!turnController || !hitImage) return;

        float w = meterRect.rect.width; // ✅ echte Pixelbreite, auch bei Stretch
        float goodPx = turnController.goodWindow * w * 2;
        Debug.Log($"meterRect width={w}, goodWindow={turnController.goodWindow} => hitPx={goodPx}");

        hitImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, goodPx);
    }

    private void Update()
    {
        if (!turnController) { Debug.LogWarning("FightUI: turnController missing"); return; }
        if (!bar) { Debug.LogWarning("FightUI: bar missing"); return; }
        bar.value = Mathf.Clamp01(turnController.Cursor01);
    }


    public void Show()
    {
        FadeIn(0.3f);
        Debug.Log("WHY IS IT NOT ACTIVE???");
    }
    public void Hide(TimingResult result)
    {
        BlinkResult(result);
        FadeOut(0.3f);
    }

    public void FadeIn(float duration = 0.15f)
    {
        LeanTween.cancel(hitPanel.gameObject);

        hitPanel.alpha = 0f;
        hitPanel.gameObject.SetActive(true);

        LeanTween.alphaCanvas(hitPanel, 1f, duration)
                 .setEaseOutQuad();
    }
    public void FadeOut(float duration = 0.15f)
    {
        LeanTween.cancel(hitPanel.gameObject);

        LeanTween.alphaCanvas(hitPanel, 0f, duration)
                 .setEaseInQuad()
                 .setOnComplete(() =>
                 {
                     hitPanel.gameObject.SetActive(false);
                 });
    }

    void BlinkResult(TimingResult result)
    {
        if (!bar) return;

        Color flashColor = result switch
        {
            TimingResult.Miss => Color.red,
            TimingResult.Hit => Color.green,
            _ => originalNormalColor
        };

        // laufende Tweens killen
        LeanTween.cancel(bar.gameObject);

        // 1) sofort Farbe setzen
        var colors = bar.colors;
        colors.normalColor = flashColor;
        bar.colors = colors;

        // 2) nach kurzer Zeit komplett resetten
        LeanTween.delayedCall(bar.gameObject, 0.12f, () =>
        {
            var colors = bar.colors;
            colors.normalColor = Color.white;
            bar.colors = colors;
        });
    }


    public void ShowResultPanel(bool win)
    {

        String result;
        if (win)
        {
            result = "WIN";
        }
        else
        {
            result = "LOSE";
        }


        WinLose.SetText(result);

        LeanTween.cancel(WinLosePanel.gameObject);

        WinLosePanel.alpha = 0f;
        WinLosePanel.gameObject.SetActive(true);

        LeanTween.alphaCanvas(WinLosePanel, 1f, 0.5f)
                 .setEaseOutQuad();
    } 

}
