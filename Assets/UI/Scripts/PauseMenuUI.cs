using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Root")]
    [SerializeField] private GameObject pauseScreen;

    // Optional: if you want the game to freeze
    [Header("Pause Behavior")]
    [SerializeField] private bool freezeTime = true;

    private bool isPaused;

    private void Awake()
    {
        if (pauseScreen != null)
            pauseScreen.SetActive(false);

        isPaused = false;

        // Safety: ensure game starts unpaused
        if (freezeTime) Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

        if (freezeTime) Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        Debug.Log("Quit to menu pressed (wire to scene load later)");
        // Later: SceneManager.LoadScene("MainMenu");
   
    }
    public void OpenOptions()
{
    Debug.Log("Options pressed (hook your Options UI here)");
}

}
