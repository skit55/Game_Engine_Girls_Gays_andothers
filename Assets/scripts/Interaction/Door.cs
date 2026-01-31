using UnityEngine;

public class Door : MonoBehaviour
{
    public ItemData requiredKey;

    public void TryOpen(GameObject player)
    {
        Inventory inventory = player.GetComponent<Inventory>();

        if (inventory != null && inventory.HasItem(requiredKey))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Door locked – Missing Key");
        }
    }

    void OpenDoor()
    {
        Debug.Log("Tür geöffnet!");
        gameObject.SetActive(false);
    }
}
