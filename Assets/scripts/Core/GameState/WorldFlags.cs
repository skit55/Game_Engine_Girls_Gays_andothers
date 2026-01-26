using System.Collections.Generic;
using UnityEngine;

public class WorldFlags : MonoBehaviour
{
    public static WorldFlags Instance { get; private set; }

    HashSet<string> defeated = new HashSet<string>();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void DebugPrintFlags()
    {
                Debug.Log("Defeated Enemies:");
        foreach (var id in defeated)
        {
            Debug.Log($"- {id}");
        }

    }
    public bool IsDefeated(string id) => defeated.Contains(id);
    public void SetDefeated(string id) => defeated.Add(id);
}
