using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteFlash : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] float flashTime = 0.25f;

    [Header("Color Flash")]
    [SerializeField] Color flashColor = Color.red;

    [Header("Flash Curve (0..1 time → 0..1 lerp)")]
    [SerializeField]
    AnimationCurve flashCurve =
        AnimationCurve.Linear(0f, 0f, 1f, 1f);

    UnityEngine.Rendering.Universal.Vignette vignette;

    Color originalColor;
    float timer = -1f;

    void Awake()
    {
        if (!volume) volume = GetComponent<Volume>();
        if (volume == null || volume.profile == null) return;
        if (!volume.profile.TryGet(out vignette) || vignette == null) return;

        originalColor = vignette.color.value;
    }

    public void Flash()
    {
        if (vignette == null) return;
        timer = 0f;
    }

    void Update()
    {
        if (timer < 0f) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / flashTime);

        float curve = flashCurve.Evaluate(t);

        vignette.color.value =
            Color.Lerp(originalColor, flashColor, curve);

        if (t >= 1f)
        {
            vignette.color.value = originalColor;
            timer = -1f;
        }
    }
}
