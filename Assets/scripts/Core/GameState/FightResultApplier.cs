using UnityEngine;

public class FightResultApplier : MonoBehaviour
{
    void Start()
    {
        if (FightContext.Instance != null)
        {
            FightContext.Instance.FightCompleted += OnFightCompleted;
            Debug.Log("Subscribed to FightCompleted");
        }
        else
        {
                        Debug.LogError("FightResultApplier: FightContext.Instance not found in Core scene.");

        }
    }

    void OnDisable()
    {
        if (FightContext.Instance != null)
            FightContext.Instance.FightCompleted -= OnFightCompleted;
    }

    void OnFightCompleted(FightResult result)
    {
        if (result.playerWon && WorldFlags.Instance != null)
        {
            WorldFlags.Instance.SetDefeated(result.encounterId);
            Debug.Log("Result Applied");
            WorldFlags.Instance.DebugPrintFlags();
        }
    }
}
