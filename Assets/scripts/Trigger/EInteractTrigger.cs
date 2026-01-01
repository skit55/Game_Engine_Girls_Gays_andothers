using UnityEngine;

public class EInteractTrigger : MonoBehaviour, IPlayerTrigger
{
    [SerializeField] MonoBehaviour actionBehaviour;

    IInteractAction action;
    PlayerInteractor currentPlayer;
    bool armed;

    void Awake()
    {
        action = actionBehaviour as IInteractAction;
        if (action == null)
            Debug.LogError($"EInteractTrigger on {name}: actionBehaviour does not implement IInteractAction.");
    }

    public void OnPlayerEnter(PlayerInteractor player)
    {
        if (action == null) return;

        // falls ihr wirklich nie overlapped: reicht das.
        // wenn doch, lösen wir Focus-Lock später.
        currentPlayer = player;

        if (armed) return;
        armed = true;

        UIManager.Instance.ShowWorldPrompt("E " + action.PromptText);
        InputHub.Instance.InteractPressed += HandleInteractPressed;
    }

    public void OnPlayerExit(PlayerInteractor player)
    {
        if (!armed) return;

        // nur disarmen, wenn es der gleiche Player ist
        if (player != currentPlayer) return;

        InputHub.Instance.InteractPressed -= HandleInteractPressed;
        UIManager.Instance.HideWorldPrompt();

        armed = false;
        currentPlayer = null;
    }

    void HandleInteractPressed()
    {
        if (!armed || action == null || currentPlayer == null) return;

        // Optional: nur in Exploration erlauben
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        action.Execute(currentPlayer);
    }
}
