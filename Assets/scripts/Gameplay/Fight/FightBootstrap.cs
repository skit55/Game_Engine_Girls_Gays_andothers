using UnityEngine;

public class FightBootstrap : MonoBehaviour
{
    [SerializeField] FightFlowState flow;

    FightRequest req;

    void Start()
    {
        if (FightContext.Instance == null)
        {
            Debug.LogError("FightBootstrap: FightContext missing.");
            return;
        }

        req = FightContext.Instance.ConsumeRequest();
        flow.BeginFight(req.enemy);

        // flow muss am Ende ein Event feuern (siehe nächster Block)
        flow.FightFinished += OnFightFinished;
    }

    void OnDestroy()
    {
        if (flow != null) flow.FightFinished -= OnFightFinished;
    }

    void OnFightFinished(bool playerWon)
    {
        // Result publishen
        FightContext.Instance.PublishResult(new FightResult
        {
            playerWon = playerWon,
            encounterId = req.encounterId,
            enemy = req.enemy
        });
        Debug.Log("FightBootstrap: Fight finished. Player won: " + playerWon);
        // Zurück in Content
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadContentScene(req.returnScene, req.returnSpawnId);
        }

        // Und GameState zurück
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Exploration);
    }
}
