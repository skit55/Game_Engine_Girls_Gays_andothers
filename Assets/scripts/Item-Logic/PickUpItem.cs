using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData item;

    void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(item);
            Destroy(gameObject); // Schlüssel verschwindet
        }
    }
}
