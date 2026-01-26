using UnityEngine;

public class PlayerTriggerSensor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var triggers = other.GetComponentsInParent<ITrigger>();
        if (triggers == null || triggers.Length == 0) return;

        for (int i = 0; i < triggers.Length; i++)
            triggers[i].OnPlayerEnter(this);
    }

    void OnTriggerExit(Collider other)
    {
        var triggers = other.GetComponentsInParent<ITrigger>();
        if (triggers == null || triggers.Length == 0) return;

        for (int i = 0; i < triggers.Length; i++)
            triggers[i].OnPlayerExit(this);
    }
}
