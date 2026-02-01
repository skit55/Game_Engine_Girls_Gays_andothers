using System.Collections.Generic;
using UnityEngine;

public class WorldFlags : MonoBehaviour
{
    public static WorldFlags Instance { get; private set; }

    HashSet<string> defeated = new HashSet<string>();
    HashSet<string> completedDialogues = new HashSet<string>();

    

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

        Debug.Log("Completed Dialogues:");
        foreach (var id in completedDialogues)
        {
            Debug.Log($"- {id}");
        }
    }

    // Enemy Flags
    public bool IsDefeated(string id) => defeated.Contains(id);
    public void SetDefeated(string id) => defeated.Add(id);

    // Dialogue Flags
    public bool IsDialogueCompleted(string id) => completedDialogues.Contains(id);
    public void SetDialogueCompleted(string id) => completedDialogues.Add(id);
}