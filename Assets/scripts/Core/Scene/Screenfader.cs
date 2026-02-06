using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] CanvasGroup group;
    [SerializeField] float fadeOutTime = 0.15f;
    [SerializeField] float fadeInTime = 0.15f;

    void Awake()
    {
        if (!group) group = GetComponentInChildren<CanvasGroup>(true);

        // Start transparent
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public Coroutine FadeOut() => StartCoroutine(FadeTo(1f, fadeOutTime));
    public Coroutine FadeIn() => StartCoroutine(FadeTo(0f, fadeInTime));

    IEnumerator FadeTo(float target, float duration)
    {
        if (!group) yield break;

        float start = group.alpha;
        float t = 0f;

        bool blocking = target > 0f;
        group.blocksRaycasts = blocking;
        group.interactable = false;

        if (duration <= 0f)
        {
            group.alpha = target;
            group.blocksRaycasts = blocking;
            yield break;
        }

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // wichtig: unabhängig von Time.timeScale
            float a = Mathf.Clamp01(t / duration);
            group.alpha = Mathf.Lerp(start, target, a);
            yield return null;
        }

        group.alpha = target;
        group.blocksRaycasts = blocking;
    }
}
