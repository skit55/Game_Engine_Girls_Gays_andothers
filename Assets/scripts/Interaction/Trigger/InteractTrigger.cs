using UnityEngine;

public class InteractTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] MonoBehaviour actionBehaviour;

    IAction action;
    PlayerTriggerSensor currentPlayer;
    bool armed;

    void Awake()
    {
        action = actionBehaviour as IAction;
        if (action == null)
            Debug.LogError($"InteractTrigger on {name}: actionBehaviour does not implement IAction.");
    }

    public void OnPlayerEnter(PlayerTriggerSensor player)
    {
        if (action == null) return;

        currentPlayer = player;

        // nur in Exploration aktivieren
        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        Arm();
    }

    public void OnPlayerExit(PlayerTriggerSensor player)
    {
        if (player != currentPlayer) return;
        Disarm();
    }

    void OnDisable() => Disarm();
    void OnDestroy() => Disarm();

    void Arm()
    {
        if (armed) return;
        armed = true;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowWorldPrompt("E " + action.PromptText);

        if (InputHub.Instance != null)
            InputHub.Instance.InteractPressed += HandleInteractPressed;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged += HandleStateChanged;
    }

    void Disarm()
    {
        if (!armed) return;
        armed = false;

        if (InputHub.Instance != null)
            InputHub.Instance.InteractPressed -= HandleInteractPressed;

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged -= HandleStateChanged;

        if (UIManager.Instance != null)
            UIManager.Instance.HideWorldPrompt();

        currentPlayer = null;
    }

    void HandleStateChanged(GameState state)
    {
        // Exploration verlassen → sofort deaktivieren
        if (state != GameState.Exploration)
        {
            Disarm();
            return;
        }

        // zurück in Exploration: falls Player noch drin steht, wieder arm
        if (currentPlayer != null && !armed)
            Arm();
    }

    void HandleInteractPressed()
    {
        if (!armed || action == null || currentPlayer == null) return;

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        action.Execute(currentPlayer);
    }
}
