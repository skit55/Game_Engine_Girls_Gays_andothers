using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuToggle : MonoBehaviour
{
    [Header("Pause UI Root")]
    [SerializeField] GameObject pauseMenuRoot;
    [SerializeField] CanvasGroup pauseGroup; // CanvasGroup AUF dem PauseMenuRoot (oder Parent)

    [Header("Options UI")]
    [SerializeField] GameObject optionsRoot;
    [SerializeField] Slider volumeSlider;

    [Header("Pause Fade")]
    [SerializeField] float fadeInTime = 0.12f;   // menu appears
    [SerializeField] float fadeOutTime = 0.10f;  // menu disappears

    [Header("Audio")]
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] string masterVolumeParam = "MasterVolume";
    [SerializeField] float minDb = -80f;

    [Header("Optional: disable these while paused")]
    [SerializeField] MonoBehaviour[] disableWhilePaused;

    [Header("Cursor")]
    [SerializeField] bool showCursorWhenPaused = true;

    [Header("Main Menu")]
    [SerializeField] string mainMenuSceneName = "MainMenu";

    bool paused;
    bool transitioning;
    Coroutine routine;

    void Awake()
    {
        if (!pauseGroup && pauseMenuRoot != null)
            pauseGroup = pauseMenuRoot.GetComponentInChildren<CanvasGroup>(true);

        // init UI hidden
        if (pauseMenuRoot != null) pauseMenuRoot.SetActive(true); // allow fade even if it starts hidden
        ApplyGroup(visible: false);

        if (optionsRoot != null) optionsRoot.SetActive(false);

        SyncSliderFromMixer();
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(SetMasterVolume01);
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(SetMasterVolume01);
    }

    void Update()
    {
        if (transitioning) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            SetPaused(!paused);
    }

    public void SetPaused(bool value)
    {
        if (transitioning) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(PauseRoutine(value));
    }

    IEnumerator PauseRoutine(bool toPaused)
    {
        transitioning = true;

        if (toPaused)
        {
            // enable root so it can fade in
            if (pauseMenuRoot != null) pauseMenuRoot.SetActive(true);

            // stop gameplay BEFORE showing menu (your choice; you can swap order if you prefer)
            EnterPauseGameplayState();

            yield return FadeCanvasGroup(pauseGroup, 1f, fadeInTime);

            // once fully visible, block clicks
            SetGroupInteraction(true);
        }
        else
        {
            // close options when leaving pause
            if (optionsRoot != null) optionsRoot.SetActive(false);

            // while fading out, don't block clicks
            SetGroupInteraction(false);

            yield return FadeCanvasGroup(pauseGroup, 0f, fadeOutTime);

            ExitPauseGameplayState();

            // optionally deactivate root after fade out
            if (pauseMenuRoot != null) pauseMenuRoot.SetActive(false);
        }

        paused = toPaused;
        transitioning = false;
        routine = null;
    }

    void EnterPauseGameplayState()
    {
        Time.timeScale = 0f;

        if (disableWhilePaused != null)
        {
            for (int i = 0; i < disableWhilePaused.Length; i++)
                if (disableWhilePaused[i] != null)
                    disableWhilePaused[i].enabled = false;
        }

        if (showCursorWhenPaused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void ExitPauseGameplayState()
    {
        Time.timeScale = 1f;

        if (disableWhilePaused != null)
        {
            for (int i = 0; i < disableWhilePaused.Length; i++)
                if (disableWhilePaused[i] != null)
                    disableWhilePaused[i].enabled = true;
        }

        if (showCursorWhenPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ApplyGroup(bool visible)
    {
        if (pauseGroup == null) return;

        pauseGroup.alpha = visible ? 1f : 0f;
        pauseGroup.blocksRaycasts = visible;
        pauseGroup.interactable = visible;
    }

    void SetGroupInteraction(bool enabled)
    {
        if (pauseGroup == null) return;
        pauseGroup.blocksRaycasts = enabled;
        pauseGroup.interactable = enabled;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, float target, float duration)
    {
        if (group == null) yield break;

        float start = group.alpha;
        float t = 0f;

        if (duration <= 0f)
        {
            group.alpha = target;
            yield break;
        }

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Clamp01(t / duration);
            group.alpha = Mathf.Lerp(start, target, a);
            yield return null;
        }

        group.alpha = target;
    }

    // --- UI Buttons ---
    public void Resume() => SetPaused(false);

    public void Options()
    {
        if (!paused) SetPaused(true);
        if (optionsRoot != null) optionsRoot.SetActive(true);
        SyncSliderFromMixer();
    }

    public void ExitOptions()
    {
        if (optionsRoot != null) optionsRoot.SetActive(false);
    }

    public void BackToMainMenu()
    {
        // IMPORTANT: unpause before load
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    // --- AUDIO ---
    void SetMasterVolume01(float value01)
    {
        if (mainMixer == null) return;
        float db = (value01 <= 0.0001f) ? minDb : Mathf.Log10(value01) * 20f;
        mainMixer.SetFloat(masterVolumeParam, db);
    }

    void SyncSliderFromMixer()
    {
        if (volumeSlider == null || mainMixer == null) return;

        if (mainMixer.GetFloat(masterVolumeParam, out float db))
        {
            float value01 = (db <= minDb + 0.01f) ? 0f : Mathf.Pow(10f, db / 20f);
            volumeSlider.SetValueWithoutNotify(value01);
        }
    }
}
