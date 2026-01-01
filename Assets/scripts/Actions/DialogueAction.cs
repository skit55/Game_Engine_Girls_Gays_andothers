using UnityEngine;

public class DialogueAction : MonoBehaviour, IInteractAction
{
    public string PromptText => "Talk";

    [SerializeField] DialogueData dialogue;

    public void Execute(PlayerInteractor player)
    {
        if (dialogue == null || dialogue.lines == null || dialogue.lines.Length == 0)
            return;

        // State -> Dialogue (Player lockt ihr über euren PlayerBrain)
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Dialogue);

        UIManager.Instance.HideWorldPrompt(); // optional
        DialogueController.Instance.StartDialogue(dialogue);
    }
}
