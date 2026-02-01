using UnityEngine;

public class Door : MonoBehaviour
{
    public ItemData requiredKey;

    private bool playerInRange;
    private GameObject player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpen();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            Debug.Log("Drücke E um die Tür zu öffnen");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    void TryOpen()
    {
        Inventory inventory = player.GetComponent<Inventory>();

        if (inventory != null && inventory.HasItem(requiredKey))
        {
            OpenDoor(inventory);
        }
        else
        {
            Debug.Log("Door locked – missing key");
        }
    }

    void OpenDoor(Inventory inventory)
    {
        inventory.RemoveItem(requiredKey); // 🔑 verschwindet überm Kopf
        Debug.Log("Tür geöffnet!");
        gameObject.SetActive(false);
    }
}
