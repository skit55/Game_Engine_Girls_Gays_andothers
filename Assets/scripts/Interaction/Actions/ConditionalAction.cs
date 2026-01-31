using UnityEngine;

public class ConditionalAction : MonoBehaviour, IAction
{
    public string PromptText
    {
        get
        {
            if (!IsConditionMet())
                return lockedText;

            var action = actionToExecute as IAction;
            return action != null ? action.PromptText : "???";
        }
    }

    public enum ConditionType { DialogueCompleted, EnemyDefeated, HasItem }

    [Header("Condition")]
    [SerializeField] ConditionType conditionType = ConditionType.DialogueCompleted;
    [SerializeField] string requiredFlagId;  
    [SerializeField] ItemData requiredItem;   
    [SerializeField] bool consumeItem = true; 

    [Header("Action")]
    [SerializeField] MonoBehaviour actionToExecute; 

    [Header("UI")]
    [SerializeField] string lockedText = "Locked (Key required)";  

    [Header("Blocking (Optional)")]
    [SerializeField] BlockMode blockMode = BlockMode.None;
    [SerializeField] GameObject blockingCollider;  
    [SerializeField] GameObject visualBlocker;     

    public enum BlockMode { None, DisableCollider, HideVisual }

    private Inventory cachedInventory;
    private bool isPermanentlyUnlocked = false; // <-- NEU: Speichert, ob die Tür offen ist

    void Start()
    {
        UpdateBlocking();
    }

    public void Execute(PlayerTriggerSensor player)
    {
        // Wenn bereits offen, einfach ausführen
        if (isPermanentlyUnlocked)
        {
            ExecuteInnerAction(player);
            return;
        }

        cachedInventory = player.GetComponentInParent<Inventory>();

        if (!IsConditionMet())
        {
            Debug.Log("Schlüssel fehlt!");
            return;
        }

        // Erfolg!
        if (conditionType == ConditionType.HasItem && consumeItem && cachedInventory != null)
        {
            cachedInventory.RemoveItem(requiredItem);
        }

        isPermanentlyUnlocked = true; // <-- NEU: Jetzt ist die Bedingung für immer erfüllt
        UpdateBlocking(); // Sofort Collider updaten
        ExecuteInnerAction(player);
    }

    private void ExecuteInnerAction(PlayerTriggerSensor player)
    {
        var action = actionToExecute as IAction;
        if (action != null) action.Execute(player);
    }

    private bool IsConditionMet()
    {
        if (isPermanentlyUnlocked) return true; // <-- NEU

        if (conditionType == ConditionType.HasItem)
        {
            if (cachedInventory == null) cachedInventory = FindObjectOfType<Inventory>();
            return cachedInventory != null && cachedInventory.HasItem(requiredItem);
        }

        // WorldFlags Logik...
        if (WorldFlags.Instance == null) return false;
        switch (conditionType)
        {
            case ConditionType.DialogueCompleted: return WorldFlags.Instance.IsDialogueCompleted(requiredFlagId);
            case ConditionType.EnemyDefeated: return WorldFlags.Instance.IsDefeated(requiredFlagId);
            default: return false;
        }
    }

    private void UpdateBlocking()
    {
        bool conditionMet = isPermanentlyUnlocked || IsConditionMet(); // <-- Geändert

        if (blockMode == BlockMode.DisableCollider && blockingCollider != null)
            blockingCollider.SetActive(!conditionMet);

        if (blockMode == BlockMode.HideVisual && visualBlocker != null)
            visualBlocker.SetActive(!conditionMet);
    }

    void Update()
    {
        if (blockMode != BlockMode.None && !isPermanentlyUnlocked) // Performance: nur Updaten wenn noch zu
        {
            UpdateBlocking();
        }
    }
}