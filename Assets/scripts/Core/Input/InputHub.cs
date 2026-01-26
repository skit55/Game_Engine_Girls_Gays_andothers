using System;
using UnityEngine;

public class InputHub : MonoBehaviour
{
    public static InputHub Instance { get; private set; }

    public event Action InteractPressed;
    public event Action AdvancePressed;


    // Fight
    public event Action<Direction> ParryPressed;
    public event Action AttackPressed;


    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // Optional: DontDestroyOnLoad(gameObject); (wenn Core persistent ist)
    }

    void Update()
    {
        var gsm = GameStateManager.Instance;
        var state = gsm != null ? gsm.CurrentState : GameState.Exploration;

        if (state == GameState.Exploration)
        {
            if (Input.GetKeyDown(KeyCode.E))
                InteractPressed?.Invoke();
        }

        if (state == GameState.Dialogue || state == GameState.Fight)
        {
            if (Input.GetKeyDown(KeyCode.E))
                AdvancePressed?.Invoke();
        }

        if (state == GameState.Fight)
        {
            // WASD + Arrows (4 Richtungen)
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                ParryPressed?.Invoke(Direction.Up);

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                ParryPressed?.Invoke(Direction.Right);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                ParryPressed?.Invoke(Direction.Left);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                ParryPressed?.Invoke(Direction.Down);
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
                AttackPressed?.Invoke();
        }
    }

    // UI hooks (optional)
    public void RaiseParry(Direction dir)
    {
        var gsm = GameStateManager.Instance;
        if (gsm != null && gsm.CurrentState == GameState.Fight)
            ParryPressed?.Invoke(dir);
    }
}
