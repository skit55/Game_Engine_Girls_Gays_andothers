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

    public enum ConditionType
    {
        DialogueCompleted,
        EnemyDefeated,
        HasItem
    }

    public enum BlockMode
    {
        None,
        DisableCollider,
        HideVisual
    }

    [Header("Condition")]
    [SerializeField] ConditionType conditionType = ConditionType.DialogueCompleted;

    [Tooltip("DialogueID oder EncounterID")]
    [SerializeField] string requiredFlagId;

    [SerializeField] ItemData requiredItem;
    [SerializeField] bool consumeItem = true;

    [Header("Action")]
    [SerializeField] MonoBehaviour actionToExecute;

    [Header("UI")]
    [SerializeField] string lockedText = "Locked";

    [Header("Blocking (Optional)")]
    [SerializeField] BlockMode blockMode = BlockMode.None;
    [SerializeField] GameObject blockingCollider;
    [SerializeField] GameObject visualBlocker;

    private Inventory cachedInventory;
    private bool isPermanentlyUnlocked;

    void Start()
    {
        // 🔥 Auto-Bind EncounterId aus FightAction
        if (conditionType == ConditionType.EnemyDefeated && string.IsNullOrEmpty(requiredFlagId))
        {
            var fight = actionToExecute as FightAction;
            if (fight != null)
            {
                requiredFlagId = fight.EncounterId;
            }
        }

        UpdateBlocking();
    }

    public void Execute(PlayerTriggerSensor player)
    {
        if (isPermanentlyUnlocked)
        {
            ExecuteInnerAction(player);
            return;
        }

        cachedInventory = player.GetComponentInParent<Inventory>();

        if (!IsConditionMet())
        {
            Debug.Log($"[ConditionalAction] Condition not met: {conditionType} ({requiredFlagId})");
            return;
        }

        if (conditionType == ConditionType.HasItem && consumeItem && cachedInventory != null)
        {
            cachedInventory.RemoveItem(requiredItem);
        }

        isPermanentlyUnlocked = true;
        UpdateBlocking();
        ExecuteInnerAction(player);
    }

    private void ExecuteInnerAction(PlayerTriggerSensor player)
    {
        var action = actionToExecute as IAction;
        action?.Execute(player);
    }

    private bool IsConditionMet()
{
    if (isPermanentlyUnlocked)
        return true;

    if (conditionType == ConditionType.HasItem)
    {
        if (cachedInventory == null)
            cachedInventory = FindObjectOfType<Inventory>();

        return cachedInventory != null && cachedInventory.HasItem(requiredItem);
    }

    if (WorldFlags.Instance == null)
        return false;

    switch (conditionType)
    {
        case ConditionType.DialogueCompleted:
            return WorldFlags.Instance.IsDialogueCompleted(requiredFlagId);

        case ConditionType.EnemyDefeated:
            return WorldFlags.Instance.IsDefeated(requiredFlagId);
    }

    return false;
}


    private void UpdateBlocking()
    {
        bool conditionMet = isPermanentlyUnlocked || IsConditionMet();

        if (blockMode == BlockMode.DisableCollider && blockingCollider != null)
            blockingCollider.SetActive(!conditionMet);

        if (blockMode == BlockMode.HideVisual && visualBlocker != null)
            visualBlocker.SetActive(!conditionMet);
    }

    void Update()
    {
        if (blockMode != BlockMode.None && !isPermanentlyUnlocked)
            UpdateBlocking();
    }
}
