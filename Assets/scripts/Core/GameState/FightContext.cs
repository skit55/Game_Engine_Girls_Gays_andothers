using System;
using UnityEngine;
public struct FightRequest
{
    public EnemyData enemy;

    // Wohin nach dem Fight?
    public string returnScene;     // z.B. "Street"
    public string returnSpawnId;   // z.B. "StreetEnemy1"

    // Welchen NPC / Encounter betrifft das?
    public string encounterId;     // z.B. "StreetEnemy1" oder eigener UniqueId
}

public struct FightResult
{
    public bool playerWon;
    public string encounterId;
    public EnemyData enemy;
}

public class FightContext : MonoBehaviour
{
    public static FightContext Instance { get; private set; }

    public FightRequest? CurrentRequest { get; private set; }
    public event Action<FightResult> FightCompleted;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetRequest(FightRequest req) => CurrentRequest = req;

    public FightRequest ConsumeRequest()
    {
        if (CurrentRequest == null)
        {
            Debug.LogError("FightContext: No request set.");
            return default;
        }

        var req = CurrentRequest.Value;
        CurrentRequest = null;
        return req;
    }

    public void PublishResult(FightResult result)
    {
        FightCompleted?.Invoke(result);
    }
}
