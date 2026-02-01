using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Stored Items")]
    public List<ItemData> items = new List<ItemData>();

    [Header("Key Visual Setup")]
    [Tooltip("Anchor über dem Player-Kopf")]
    public Transform keyAnchor;

    [Tooltip("Prefab des schwebenden Keys")]
    public GameObject keyVisual;

    [Tooltip("Das ItemData, das als Schlüssel gilt (z.B. GoldKey)")]
    public ItemData keyItem;

    private GameObject currentKeyVisual;

    // =============================
    // ITEM HINZUFÜGEN
    // =============================
    public void AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("AddItem called with NULL item");
            return;
        }

        if (items.Contains(item))
        {
            Debug.Log("Item bereits im Inventar: " + item.name);
            return;
        }

        items.Add(item);
        Debug.Log("Item aufgenommen: " + item.name);

        // Key-Visual anzeigen
        if (item == keyItem)
        {
            ShowKeyVisual();
        }
    }

    // =============================
    // ITEM ABFRAGEN
    // =============================
    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }

    // =============================
    // ITEM ENTFERNEN (z.B. nach Tür)
    // =============================
    public void RemoveItem(ItemData item)
    {
        if (!items.Contains(item))
            return;

        items.Remove(item);
        Debug.Log("Item entfernt: " + item.name);

        if (item == keyItem)
        {
            HideKeyVisual();
        }
    }

    // =============================
    // KEY VISUAL SPAWNEN
    // =============================
void ShowKeyVisual()
{
    if (currentKeyVisual != null)
        return;

    currentKeyVisual = Instantiate(
        keyVisual,
        keyAnchor.position,
        Quaternion.identity,
        keyAnchor
    );

    currentKeyVisual.transform.localPosition = Vector3.zero;
}

void HideKeyVisual()
{
    if (currentKeyVisual != null)
    {
        Destroy(currentKeyVisual);
        currentKeyVisual = null;
    }
}

}
