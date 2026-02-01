using UnityEngine;

public class FightAction : MonoBehaviour, IAction
{
    public string PromptText => "Fight";

    [SerializeField] EnemyData enemy;
    [SerializeField] string encounterId = "StreetEnemy1";
    [SerializeField] string returnSpawnId = "StreetEnemy1";

    public void Execute(PlayerTriggerSensor player)
    {
        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        if (FightContext.Instance == null)
        {
            Debug.LogError("FightAction: FightContext.Instance not found in Core scene.");
            return;
        }

        if (SceneLoader.Instance == null)
        {
            Debug.LogError("FightAction: SceneLoader.Instance not found.");
            return;
        }

        if (WorldFlags.Instance != null && WorldFlags.Instance.IsDefeated(encounterId)) {
            Debug.Log("ALREADY DEFEAT THIS FOOL");
            return;
        }

        // Return-Scene aus SceneLoader holen (Property ggf. ergänzen)
        string returnScene = SceneLoader.Instance.CurrentContentSceneName; // <- Property anlegen

        FightContext.Instance.SetRequest(new FightRequest
        {
            enemy = enemy,
            returnScene = returnScene,
            returnSpawnId = returnSpawnId,
            encounterId = encounterId
        });

        GameStateManager.Instance.SetState(GameState.Fight);
        SceneLoader.Instance.LoadContentScene("Fight", "FightStart");
        SfxManager.Instance.PlaySFX2D(SfxBank.Instance.fightEnter);

    }
}