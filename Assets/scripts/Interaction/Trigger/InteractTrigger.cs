using UnityEngine;

public class InteractTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] MonoBehaviour actionBehaviour;

    IAction Action
    {
        get
        {
            if (_action == null && actionBehaviour != null)
                _action = actionBehaviour as IAction;

            return _action;
        }
    }

    IAction _action;
    PlayerTriggerSensor currentPlayer;
    bool armed;

    void OnValidate()
    {
        if (actionBehaviour != null && !(actionBehaviour is IAction))
        {
            Debug.LogError(
                $"InteractTrigger on {name}: Assigned actionBehaviour does not implement IAction!",
                this
            );
        }
    }

    public void OnPlayerEnter(PlayerTriggerSensor player)
    {
        
        Debug.Log($"[InteractTrigger] Player entered {name}");
        if (Action == null) return;

        currentPlayer = player;

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        Arm();
        SfxManager.Instance.PlaySFX2D(SfxBank.Instance.dialogue);
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
            UIManager.Instance.ShowWorldPrompt("E " + Action.PromptText);

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
        if (state != GameState.Exploration)
        {
            Disarm();
            return;
        }

        if (currentPlayer != null && !armed)
            Arm();
    }

    void HandleInteractPressed()
    {
        if (!armed || Action == null || currentPlayer == null) return;

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        Action.Execute(currentPlayer);
    }
}
