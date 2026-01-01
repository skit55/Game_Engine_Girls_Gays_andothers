using System;
using UnityEngine;

public class InputHub : MonoBehaviour
{
    public static InputHub Instance { get; private set; }

    public event Action InteractPressed; // E
    public event Action AdvancePressed;  // Space (Dialogue skip/next)

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        // Interact (Exploration)
        if (Input.GetKeyDown(KeyCode.E))
            InteractPressed?.Invoke();

        // Advance (Dialogue)
        if (Input.GetKeyDown(KeyCode.Space))
            AdvancePressed?.Invoke();
    }
}
