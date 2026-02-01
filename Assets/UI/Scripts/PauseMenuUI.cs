using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject optionsScreen;

    [Header("Pause Behavior")]
    [SerializeField] private bool freezeTime = true;

    [Header("Options: Digit Sprites (0–9)")]
    [SerializeField] private Sprite[] digitSprites; // size 10, index = number

    [Header("Options: Value Images")]
    [SerializeField] private Image musicValueImage;
    [SerializeField] private Image soundValueImage;

    [Header("Options: Values")]
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 9;
    [SerializeField] private int musicValue = 6;
    [SerializeField] private int soundValue = 7;

    private bool isPaused;

    private void Awake()
    {
        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (optionsScreen != null) optionsScreen.SetActive(false);

        isPaused = false;

        if (freezeTime) Time.timeScale = 1f;

        RefreshOptionsUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If options is open, Esc should go back to pause menu
            if (optionsScreen != null && optionsScreen.activeSelf)
            {
                CloseOptions();
                return;
            }

            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (pauseScreen == null)
        {
            Debug.LogError("PauseMenuUI: pauseScreen is not assigned.");
            return;
        }

        isPaused = true;
        pauseScreen.SetActive(true);

        if (freezeTime) Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (pauseScreen == null)
        {
            Debug.LogError("PauseMenuUI: pauseScreen is not assigned.");
            return;
        }

        isPaused = false;

        pauseScreen.SetActive(false);
        if (optionsScreen != null) optionsScreen.SetActive(false);

        if (freezeTime) Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        Debug.Log("Quit to menu pressed (wire to scene load later)");
        // Later: SceneManager.LoadScene("MainMenu");
    }

    // -------- OPTIONS SCREEN --------

    public void OpenOptions()
    {
        if (optionsScreen == null)
        {
            Debug.LogError("PauseMenuUI: optionsScreen is not assigned.");
            return;
        }

        optionsScreen.SetActive(true);
        if (pauseScreen != null) pauseScreen.SetActive(false);

        RefreshOptionsUI();
    }

    public void CloseOptions()
    {
        if (optionsScreen != null) optionsScreen.SetActive(false);
        if (pauseScreen != null) pauseScreen.SetActive(true);
    }

    // MUSIC buttons
    public void MusicUp()
    {
        musicValue = Mathf.Clamp(musicValue + 1, minValue, maxValue);
        RefreshOptionsUI();
        Debug.Log($"Music volume: {musicValue}");
    }

    public void MusicDown()
    {
        musicValue = Mathf.Clamp(musicValue - 1, minValue, maxValue);
        RefreshOptionsUI();
        Debug.Log($"Music volume: {musicValue}");
    }

    // SOUND/SFX buttons
    public void SoundUp()
    {
        soundValue = Mathf.Clamp(soundValue + 1, minValue, maxValue);
        RefreshOptionsUI();
        Debug.Log($"Sound volume: {soundValue}");
    }

    public void SoundDown()
    {
        soundValue = Mathf.Clamp(soundValue - 1, minValue, maxValue);
        RefreshOptionsUI();
        Debug.Log($"Sound volume: {soundValue}");
    }

    private void RefreshOptionsUI()
    {
        // Don’t spam errors while building UI; just fail gracefully.
        if (digitSprites == null || digitSprites.Length < 10) return;

        if (musicValueImage != null)
            musicValueImage.sprite = digitSprites[musicValue];

        if (soundValueImage != null)
            soundValueImage.sprite = digitSprites[soundValue];
    }
}
