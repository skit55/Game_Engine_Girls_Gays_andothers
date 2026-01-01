using UnityEngine;
using TMPro;

public class WorldPrompt : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI label;

    void Awake()
    {
        Hide(); // Start unsichtbar
    }

    public void Show(string text)
    {
        if (label != null)
            label.text = text;

        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
    }

    void SetVisible(bool visible)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
        else
        {
            gameObject.SetActive(visible);
        }
    }
}
