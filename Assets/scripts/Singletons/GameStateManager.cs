using System;
using UnityEngine;

public enum GameState { Exploration, Dialogue, Fight }

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [SerializeField] GameState currentState = GameState.Exploration;
    public GameState CurrentState => currentState;

    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        Debug.Log($"GameState → {currentState}");
        OnStateChanged?.Invoke(currentState);
    }

    public bool CanPlayerMove() => currentState == GameState.Exploration;
}
