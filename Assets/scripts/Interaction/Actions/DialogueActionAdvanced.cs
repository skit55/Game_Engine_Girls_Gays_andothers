using UnityEngine;

/// <summary>
/// Advanced DialogueAction mit Dialog-Progression und Flag-System.
/// Für NPCs die zwei verschiedene Dialoge haben (vor/nach einem Event).
/// </summary>
public class DialogueActionAdvanced : MonoBehaviour, IAction
{
    public string PromptText => "Talk";

    [Header("Dialogues")]
    [SerializeField] DialogueData defaultDialogue;        // Erster Dialog
    [SerializeField] DialogueData afterDialogueDialogue;  // Zweiter Dialog (nach Flag)

    [Header("Progress")]
    [SerializeField] string dialogueId = "NPC_FirstTalk"; // Eindeutige ID für Flag

    public void Execute(PlayerTriggerSensor player)
    {
        // Welcher Dialog wird gespielt?
        DialogueData dialogueToPlay = defaultDialogue;
        
        // Prüfe ob bereits gesprochen
        if (WorldFlags.Instance != null && 
            WorldFlags.Instance.IsDialogueCompleted(dialogueId) && 
            afterDialogueDialogue != null)
        {
            dialogueToPlay = afterDialogueDialogue;
        }

        // Validierung
        if (dialogueToPlay == null || dialogueToPlay.lines == null || dialogueToPlay.lines.Length == 0)
        {
            Debug.LogWarning($"DialogueActionAdvanced: No dialogue data for {name}");
            return;
        }

        // GameState auf Dialogue
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Dialogue);

        // Event: Nach Dialog Flag setzen
        if (DialogueController.Instance != null)
        {
            DialogueController.Instance.OnDialogueFinished -= OnDialogueFinished;
            DialogueController.Instance.OnDialogueFinished += OnDialogueFinished;
            DialogueController.Instance.StartDialogue(dialogueToPlay);
        }
        else
        {
            Debug.LogError("DialogueActionAdvanced: DialogueController.Instance not found.");
        }
    }

    void OnDisable()
    {
        if (DialogueController.Instance != null)
            DialogueController.Instance.OnDialogueFinished -= OnDialogueFinished;
    }

    void OnDestroy()
    {
        if (DialogueController.Instance != null)
            DialogueController.Instance.OnDialogueFinished -= OnDialogueFinished;
    }

    private void OnDialogueFinished()
    {
        // Flag setzen (nur beim ersten Mal)
        if (WorldFlags.Instance != null && 
            !WorldFlags.Instance.IsDialogueCompleted(dialogueId))
        {
            WorldFlags.Instance.SetDialogueCompleted(dialogueId);
            Debug.Log($"[DialogueActionAdvanced] Dialog abgeschlossen: {dialogueId}");
        }

        // Event cleanup
        if (DialogueController.Instance != null)
            DialogueController.Instance.OnDialogueFinished -= OnDialogueFinished;
    }
}