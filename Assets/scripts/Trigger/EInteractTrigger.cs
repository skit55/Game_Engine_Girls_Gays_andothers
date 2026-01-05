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

        currentPlayer = player;

        if (armed) return;
        armed = true;

        UIManager.Instance.ShowWorldPrompt("E " + action.PromptText);
        InputHub.Instance.InteractPressed += HandleInteractPressed;
    }

    public void OnPlayerExit(PlayerInteractor player)
    {
        if (player != currentPlayer) return;
        Disarm();
    }

    void OnDisable() => Disarm();   // <-- CRITICAL
    void OnDestroy() => Disarm();   // <-- CRITICAL

    void Disarm()
    {
        if (!armed) return;

        // safe unsubscribe (auch wenn InputHub beim Shutdown evtl. schon weg ist)
        if (InputHub.Instance != null)
            InputHub.Instance.InteractPressed -= HandleInteractPressed;

        if (UIManager.Instance != null)
            UIManager.Instance.HideWorldPrompt();

        armed = false;
        currentPlayer = null;
    }

    void HandleInteractPressed()
    {
        if (!armed || action == null || currentPlayer == null) return;

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        // Action ausführen
        action.Execute(currentPlayer);
    }
}
