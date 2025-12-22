using UnityEngine;


public enum GameState
{
    Exploration,
    Dialogue,
    Fight
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [SerializeField]
    GameState currentState = GameState.Exploration;

    public GameState CurrentState => currentState;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        Debug.Log($"GameState → {currentState}");
    }

    // --- Convenience Queries (wichtig!)
    public bool CanPlayerMove()
    {
        return currentState == GameState.Exploration;
    }

    public bool IsDialogue()
    {
        return currentState == GameState.Dialogue;
    }

    public bool IsFight()
    {
        return currentState == GameState.Fight;
    }
}
