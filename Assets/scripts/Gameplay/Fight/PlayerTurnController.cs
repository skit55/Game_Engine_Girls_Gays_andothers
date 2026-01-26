using System;
using UnityEngine;

public enum TimingResult { Miss, Hit }

public class PlayerTurnController : MonoBehaviour
{
   
    [Header("Timing")]
    [SerializeField, Range(0f, 1f)] float center = 0.5f;
    [SerializeField] float speed01PerSec = 1.6f;   // je höher, desto schwerer
    [SerializeField] bool pingPong = true;

    [Header("Windows (normalized distance to center)")]
    [SerializeField] public float goodWindow = 0.14f;


    public event Action<TimingResult> PlayerTurnFinished;

    public bool IsRunning => isRunning;
    
    float cursor01;
    public float Cursor01 => cursor01;


    bool isRunning;
    
    int dir = 1;
    float t;

    

    public void BeginPlayerTurn()
    {
        isRunning = true;
        cursor01 = 0f;
        dir = 1;
        t = 0f;
        InputHub.Instance.AttackPressed += OnAttack; // falls es das noch nicht gibt -> siehe unten

        // Hier optional: UI einblenden / SFX
        // FightUI.Instance?.ShowTiming(true);
        Debug.Log("BEGIN PLAYER TURN");
    }

    public void EndPlayerTurn(TimingResult result)
    {
        isRunning = false;

    }

    void Update()
    {
        if (Time.frameCount % 30 == 0)
            Debug.Log($"cursor={cursor01:0.00} speed={speed01PerSec} dir={dir}");

        if (!isRunning) return;

        // optional gate: nur im Fight state
        var gsm = GameStateManager.Instance;
        if (gsm != null && gsm.CurrentState != GameState.Fight) return;

        float dt = Time.deltaTime;

        t += dt;
        

        cursor01 += dir * speed01PerSec * dt;

        if (pingPong)
        {
            if (cursor01 >= 1f) { cursor01 = 1f; dir = -1; }
            if (cursor01 <= 0f) { cursor01 = 0f; dir = 1; }
        }

        // UI hook:
        // FightUI.Instance?.SetTimingCursor(cursor01);
    }

    void OnAttack()
    {
        Debug.Log("ATTACK");
        InputHub.Instance.AttackPressed -= OnAttack;

        if (!isRunning) return;

        // optional: allow attack only in player turn (wird eh nur bei IsRunning true passieren)
        var result = Evaluate();
        Debug.Log(result.ToString());
        Resolve(result);
    }

    TimingResult Evaluate()
    {
        float d = Mathf.Abs(cursor01 - center);

        if (d <= goodWindow) { 
            return TimingResult.Hit;
        }
        FindFirstObjectByType<CameraShakeSimple>()?.Shake();
        return TimingResult.Miss;
    }

    void Resolve(TimingResult result)
    {
        if (!isRunning) return;
        EndPlayerTurn(result);
        PlayerTurnFinished?.Invoke(result);
    }
}
