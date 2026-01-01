using UnityEngine;

public class DebugInteractAction : MonoBehaviour, IInteractAction
{
    public string PromptText => "Debug";

    public void Execute(PlayerInteractor player)
    {
        Debug.Log($"[DEBUG ACTION] Interacted with {name}");
    }
}
