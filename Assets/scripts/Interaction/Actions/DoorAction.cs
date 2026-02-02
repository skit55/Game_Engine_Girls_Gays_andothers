using UnityEngine;

public class DoorAction : MonoBehaviour, IAction
{
    public string PromptText => "Open";

    [SerializeField] string spawnId;
    [SerializeField] string sceneName;

    public void Execute(PlayerTriggerSensor player)
    {
        

        // safety: only in exploration
        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Exploration)
            return;

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadContentScene(sceneName, spawnId);
        else
            Debug.LogError("DoorAction: SceneLoader.Instance not found.");

        SfxManager.Instance.PlaySFX2D(SfxBank.Instance.door);

    }
}
