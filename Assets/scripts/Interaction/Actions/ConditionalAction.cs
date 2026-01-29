using UnityEngine;

public class ConditionalAction : MonoBehaviour, IAction
{
    public string PromptText
    {
        get
        {
            // Wenn Bedingung nicht erfüllt, zeige "Locked" an
            if (!IsConditionMet())
                return lockedText;

            // Ansonsten zeige Prompt der eigentlichen Action
            var action = actionToExecute as IAction;
            return action != null ? action.PromptText : "???";
        }
    }

    [Header("Condition")]
    [SerializeField] ConditionType conditionType = ConditionType.DialogueCompleted;
    [SerializeField] string requiredFlagId;  // ID zum Prüfen

    [Header("Action")]
    [SerializeField] MonoBehaviour actionToExecute; // Tür / Dialog / andere Aktion

    [Header("UI")]
    [SerializeField] string lockedText = "Locked";  // Text wenn nicht freigeschaltet

    [Header("Blocking (Optional)")]
    [SerializeField] BlockMode blockMode = BlockMode.None;
    [SerializeField] GameObject blockingCollider;  // Collider der deaktiviert wird
    [SerializeField] GameObject visualBlocker;     // Visuelles Element (z.B. Gitter)

    public enum ConditionType
    {
        DialogueCompleted,
        EnemyDefeated
    }

    public enum BlockMode
    {
        None,              // Nur UI-Prompt, keine physische Blockade
        DisableCollider,   // Collider deaktivieren wenn freigeschaltet
        HideVisual         // Visuelles Element verstecken wenn freigeschaltet
    }

    void Start()
    {
        UpdateBlocking();
    }

    void OnEnable()
    {
        UpdateBlocking();
    }

    public void Execute(PlayerTriggerSensor player)
    {
        // Prüfe Bedingung
        if (!IsConditionMet())
        {
            Debug.Log($"ConditionalAction: Bedingung nicht erfüllt - {conditionType}: {requiredFlagId}");
            
            // Feedback: Locked sound oder UI message
            if (UIManager.Instance != null)
            {
                // Könnte hier eine Message anzeigen
            }
            return;
        }

        // Bedingung erfüllt - führe Action aus
        var action = actionToExecute as IAction;
        if (action != null)
        {
            Debug.Log($"ConditionalAction: Bedingung erfüllt - führe Action aus");
            action.Execute(player);
        }
        else
        {
            Debug.LogError("ConditionalAction: actionToExecute is not an IAction");
        }
    }

    private bool IsConditionMet()
    {
        if (WorldFlags.Instance == null)
        {
            Debug.LogError("ConditionalAction: WorldFlags.Instance not found!");
            return false;
        }

        switch (conditionType)
        {
            case ConditionType.DialogueCompleted:
                return WorldFlags.Instance.IsDialogueCompleted(requiredFlagId);
            
            case ConditionType.EnemyDefeated:
                return WorldFlags.Instance.IsDefeated(requiredFlagId);
            
            default:
                return false;
        }
    }

    private void UpdateBlocking()
    {
        bool conditionMet = IsConditionMet();

        switch (blockMode)
        {
            case BlockMode.DisableCollider:
                if (blockingCollider != null)
                {
                    // Wenn Bedingung erfüllt: Collider deaktivieren (durchgehbar)
                    // Wenn nicht erfüllt: Collider aktivieren (blockiert)
                    blockingCollider.SetActive(!conditionMet);
                }
                break;

            case BlockMode.HideVisual:
                if (visualBlocker != null)
                {
                    // Wenn Bedingung erfüllt: Visual verstecken
                    // Wenn nicht erfüllt: Visual zeigen
                    visualBlocker.SetActive(!conditionMet);
                }
                break;
        }
    }

    // Optional: Update in Play Mode wenn Flag gesetzt wird
    void Update()
    {
        if (blockMode != BlockMode.None)
        {
            UpdateBlocking();
        }
    }

    // Optional: Für Inspector-Preview
    void OnValidate()
    {
        if (actionToExecute != null && !(actionToExecute is IAction))
        {
            Debug.LogWarning($"ConditionalAction on {name}: actionToExecute does not implement IAction!");
        }
    }
}