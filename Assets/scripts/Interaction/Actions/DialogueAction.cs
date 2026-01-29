using UnityEngine;

public class DialogueAction : MonoBehaviour, IAction
{
    public string PromptText => "Talk";

    [SerializeField] DialogueData dialogue;

    public void Execute(PlayerTriggerSensor player)
    {
        if (dialogue == null || dialogue.lines == null || dialogue.lines.Length == 0)
            return;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Dialogue);

        if (DialogueController.Instance != null)
            DialogueController.Instance.StartDialogue(dialogue);
        else
            Debug.LogError("DialogueAction: DialogueController.Instance not found.");
    }
}