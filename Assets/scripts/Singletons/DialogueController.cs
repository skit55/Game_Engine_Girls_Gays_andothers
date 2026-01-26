using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    [Header("UI")]
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI speakerLabel;
    [SerializeField] TextMeshProUGUI bodyLabel;

    [Header("Choices UI")]
    [SerializeField] GameObject choicesPanel;
    [SerializeField] Button[] choiceButtons;                 // size 3
    [SerializeField] TextMeshProUGUI[] choiceButtonLabels;   // size 3 (Text components in buttons)

    [Header("Typewriter")]
    [SerializeField] float charsPerSecond = 40f;

    DialogueData current;
    int lineIndex;

    Coroutine typing;
    bool isTyping;
    string fullLine = "";

    bool choicesOpen;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        if (panel != null) panel.SetActive(false);
        if (choicesPanel != null) choicesPanel.SetActive(false);
    }

    void Update()
    {
        // Optional: 1/2/3 während Choices offen sind
        if (!choicesOpen) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) Choose(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Choose(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Choose(2);
    }

    public void StartDialogue(DialogueData data)
    {
        current = data;
        lineIndex = 0;
        choicesOpen = false;

        if (speakerLabel != null) speakerLabel.text = current.speakerName;
        if (panel != null) panel.SetActive(true);
        if (choicesPanel != null) choicesPanel.SetActive(false);

        // Input routing: Space steuert Dialog
        InputHub.Instance.AdvancePressed += OnAdvance;

        ShowLine(lineIndex);
    }

    public void EndDialogue()
    {
        // cleanup
        if (InputHub.Instance != null)
            InputHub.Instance.AdvancePressed -= OnAdvance;

        if (typing != null) StopCoroutine(typing);
        typing = null;

        isTyping = false;
        fullLine = "";
        current = null;
        choicesOpen = false;

        if (choicesPanel != null) choicesPanel.SetActive(false);
        if (panel != null) panel.SetActive(false);

        // zurück zu Exploration
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

        // Wenn Choice offen ist, ignorieren wir Space
        if (choicesOpen) return;

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

        // Wenn Dialog zu Ende ist: entweder Choices oder Ende
        if (lineIndex >= current.lines.Length)
        {
            if (current.hasChoices)
            {
                OpenChoices();
                return;
            }

            EndDialogue();
            return;
        }

        ShowLine(lineIndex);
    }

    void OpenChoices()
    {
        choicesOpen = true;

        if (choicesPanel != null)
            choicesPanel.SetActive(true);

        // Buttons setzen + onClick binden
        for (int i = 0; i < 3; i++)
        {
            bool hasEntry =
                current.choiceTexts != null && i < current.choiceTexts.Length &&
                !string.IsNullOrWhiteSpace(current.choiceTexts[i]) &&
                current.choiceNext != null && i < current.choiceNext.Length &&
                current.choiceNext[i] != null;

            if (choiceButtons != null && i < choiceButtons.Length && choiceButtons[i] != null)
            {
                choiceButtons[i].gameObject.SetActive(hasEntry);

                choiceButtons[i].onClick.RemoveAllListeners();
                int captured = i;
                if (hasEntry)
                    choiceButtons[i].onClick.AddListener(() => Choose(captured));
            }

            if (choiceButtonLabels != null && i < choiceButtonLabels.Length && choiceButtonLabels[i] != null)
            {
                choiceButtonLabels[i].text = hasEntry ? current.choiceTexts[i] : "";
            }
        }
    }

    void Choose(int index)
    {
        if (!choicesOpen || current == null) return;

        if (current.choiceNext == null || index < 0 || index >= current.choiceNext.Length) return;
        DialogueData next = current.choiceNext[index];
        if (next == null) return;

        // Choice UI schließen
        choicesOpen = false;
        if (choicesPanel != null) choicesPanel.SetActive(false);

        // Nächsten Dialog starten
        StartDialogue(next);
    }
}
