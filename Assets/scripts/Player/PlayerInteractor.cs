using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var triggers = other.GetComponentsInParent<IPlayerTrigger>();
        if (triggers == null || triggers.Length == 0) return;

        for (int i = 0; i < triggers.Length; i++)
            triggers[i].OnPlayerEnter(this);

        Debug.Log("ENTERED Interactor");
    }

    void OnTriggerExit(Collider other)
    {
        var triggers = other.GetComponentsInParent<IPlayerTrigger>();
        if (triggers == null || triggers.Length == 0) return;

        for (int i = 0; i < triggers.Length; i++)
            triggers[i].OnPlayerExit(this);
    }
}
