using UnityEngine;

public class Door : MonoBehaviour, IInteractAction
{
    public string PromptText => "Open";

    [SerializeField] public string spawnId;
    [SerializeField] public string sceneName;

    public void Execute(PlayerInteractor player)
    {
        SceneLoader.Instance.LoadContentScene(sceneName, spawnId);

    }
}
