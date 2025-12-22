using UnityEngine;

public class DebugTrigger : MonoBehaviour, IPlayerTrigger
{
    public string message = "Trigger entered!";

    public void OnPlayerEnter(PlayerInteractor player)
    {
        Debug.Log(message);
        // später: GameState -> Dialogue, UIManager.ShowDialogue(...)
    }

    public void OnPlayerExit(PlayerInteractor player)
    {
        Debug.Log("Trigger exit");
    }
}
