using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    public event System.Action OnDialogueFinished; //DialogueController bekommt ein Event 
    [Header("UI")]
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI speakerLabel;
    [SerializeField] TextMeshProUGUI bodyLabel;

    [Header("Typewriter")]
    [SerializeField] float charsPerSecond = 40f;

    DialogueData current;
    int lineIndex;

    Coroutine typing;
    bool isTyping;
    string fullLine = "";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        if (panel != null) panel.SetActive(false);
    }

    public void StartDialogue(DialogueData data)
    {
        current = data;
        lineIndex = 0;

        // Setzt die speakerName und startet das Panel
        if (speakerLabel != null) speakerLabel.text = current.speakerName;
        if (panel != null) panel.SetActive(true);

        // Input routing: Space steuert Dialog
        InputHub.Instance.AdvancePressed += OnAdvance;

        // Setzt den Dialogtext
        ShowLine(lineIndex);
    }

    public void EndDialogue()
    {
        // cleanup
        InputHub.Instance.AdvancePressed -= OnAdvance;

        if (typing != null) StopCoroutine(typing);
        typing = null;

        isTyping = false;
        fullLine = "";

        // ⚡ Flag setzen bevor current gelöscht wird
        if (current != null && !string.IsNullOrEmpty(current.DialogueID) && WorldFlags.Instance != null)
        {
            // Speichert die ID des Dialogs in den WorldFlags
            WorldFlags.Instance.SetDialogueCompleted(current.DialogueID);
            Debug.Log($"[Dialogue] Completed: {current.DialogueID}");
        }
        else
        {
            Debug.LogError("[Dialogue] DialogueID is empty or WorldFlags.Instance is null.");
        }

        current = null;

        if (panel != null) panel.SetActive(false);
        // End dialog trigger
        OnDialogueFinished?.Invoke();

        // Zurück zu Exploration
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Exploration);
    }

    void ShowLine(int idx)
    {
        if (current == null || current.lines == null || current.lines.Length == 0) return;

        fullLine = current.lines[idx];

        if (typing != null) StopCoroutine(typing);
        typing = StartCoroutine(TypeLine(fullLine));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        if (bodyLabel != null) bodyLabel.text = "";

        float delay = 1f / Mathf.Max(1f, charsPerSecond);

        for (int i = 0; i < line.Length; i++)
        {
            if (bodyLabel != null) bodyLabel.text += line[i];
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        typing = null;
    }

    void OnAdvance()
    {
        if (current == null) return;

        // 1) wenn noch tippt -> skip
        if (isTyping)
        {
            if (typing != null) StopCoroutine(typing);
            typing = null;

            if (bodyLabel != null) bodyLabel.text = fullLine;
            isTyping = false;
            return;
        }

        // 2) nächste Line / Ende
        lineIndex++;
        if (lineIndex >= current.lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine(lineIndex);
    }
}
