using System.Collections;
using UnityEngine;

public class GateAction : MonoBehaviour, IAction
{
    public string PromptText => "Open";

    [Header("Visuals")]
    [SerializeField] public CanvasGroup closedGroup;
    [SerializeField] public CanvasGroup openGroup;
    
    [Header("Physics")]
    [SerializeField] public GameObject blockingCollider; // <-- NEU: Hier die Wand/Collider rein

    [Header("Settings")]
    [Range(0f, 2f)]
    [SerializeField] public float pause;
    [Range(0.2f, 2f)]
    [SerializeField] public float speed;

    public void Execute(PlayerTriggerSensor player)
    {
        // Sobald die Action ausgeführt wird, schalten wir den Collider aus
        if (blockingCollider != null)
        {
            blockingCollider.SetActive(false);
            Debug.Log("<color=green>GateAction:</color> Collider wurde deaktiviert.");
        }

        StartCoroutine(Fade(pause, speed));
    }

    IEnumerator Fade(float pause, float speed)
    {
        // Startet das Ausfaden des geschlossenen Tors
        LeanTween.alphaCanvas(closedGroup, 0f, speed);
        
        // Wartet kurz (hier kannst du 'pause' verwenden statt der festen 1f)
        yield return new WaitForSeconds(pause);
        
        // Startet das Einfaden des offenen Tors
        LeanTween.alphaCanvas(openGroup, 1f, speed);
    }
}