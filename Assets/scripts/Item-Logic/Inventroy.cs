using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log("Item aufgenommen: " + item.itemName);
        }
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}
