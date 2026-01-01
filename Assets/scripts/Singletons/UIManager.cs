using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("World Prompt")]
    [SerializeField] WorldPrompt worldPrompt;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowWorldPrompt(string text)
    {
        if (worldPrompt == null) return;
        worldPrompt.Show(text);
    }

    public void HideWorldPrompt()
    {
        if (worldPrompt == null) return;
        worldPrompt.Hide();
    }

    // Platzhalter für später (Dialog etc.)
    public void ShowDialogue(string text)
    {
        Debug.Log("[UI] Dialogue: " + text);
    }
}
