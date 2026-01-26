using UnityEngine;

public class DebugAction : MonoBehaviour, IAction
{
    public string PromptText => "Debug";

    public void Execute(PlayerTriggerSensor player)
    {
        Debug.Log($"[DEBUG ACTION] Interacted with {name}");
    }
}
