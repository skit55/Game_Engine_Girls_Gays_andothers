using UnityEngine;

public class QuickDiagnose : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                // 1. Prüfe ob WorldFlags existiert
        Debug.Log(WorldFlags.Instance != null);

        // 2. Zeige alle Flags
        if (WorldFlags.Instance != null)
            WorldFlags.Instance.DebugPrintFlags();

        // 3. Prüfe spezifisches Flag
        if (WorldFlags.Instance != null)
            Debug.Log(WorldFlags.Instance.IsDialogueCompleted("test-dialogue"));

        // 4. Finde Pigeon und zeige PromptText
        var pigeon = GameObject.Find("Pigeon");
        if (pigeon != null)
        {
            var conditional = pigeon.GetComponent<ConditionalAction>();
            if (conditional != null)
                Debug.Log("PromptText: " + conditional.PromptText);
        }
    }
}
