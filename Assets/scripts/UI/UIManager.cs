using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("World Prompt")]
    [SerializeField] WorldPrompt worldPrompt;

    [Header("Panels")]
    [SerializeField] GameObject fightPanel;
    // später:
    // [SerializeField] GameObject dialoguePanel;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("[UIManager] GameStateManager not found.");
            return;
        }

        GameStateManager.Instance.OnStateChanged += ApplyState;
        ApplyState(GameStateManager.Instance.CurrentState);
    }

    void OnDestroy()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged -= ApplyState;
    }

    void ApplyState(GameState state)
    {
        // WorldPrompt nur in Exploration
        if (state != GameState.Exploration)
            HideWorldPrompt();

        // Fight panel
        if (fightPanel != null)
            fightPanel.SetActive(state == GameState.Fight);

        // später:
        // if (dialoguePanel != null)
        //     dialoguePanel.SetActive(state == GameState.Dialogue);
    }

    public void ShowWorldPrompt(string text)
    {
        // safety: prompt only in exploration
        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        if (worldPrompt == null) return;
        worldPrompt.Show(text);
    }

    public void HideWorldPrompt()
    {
        if (worldPrompt == null) return;
        worldPrompt.Hide();
    }

    // placeholder: später durch echtes DialogueUI ersetzen
    public void ShowDialogue(string text)
    {
        Debug.Log("[UI] Dialogue: " + text);
    }
}
