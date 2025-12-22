using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Tooltip("Optional: verhindert Spam, wenn man mehrfach schnell rein/raus jittert.")]
    public float enterCooldown = 0.2f;

    float lastEnterTime;

    void OnTriggerEnter(Collider other)
    {
        // Nur interagieren, wenn wir wirklich im Exploration-Mode sind
        if (GameStateManager.Instance != null && !GameStateManager.Instance.CanPlayerMove())
            return;

        // Cooldown gegen Trigger-Jitter (z.B. an Kanten)
        if (Time.time - lastEnterTime < enterCooldown)
            return;

        var trigger = other.GetComponent<IPlayerTrigger>();
        if (trigger == null) return;

        lastEnterTime = Time.time;
        trigger.OnPlayerEnter(this);
    }

    void OnTriggerExit(Collider other)
    {
        var trigger = other.GetComponent<IPlayerTrigger>();
        if (trigger == null) return;

        trigger.OnPlayerExit(this);
    }
}
