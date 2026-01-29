using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Helper Component um Door + ConditionalAction Setup zu debuggen.
/// Zeigt alle Probleme direkt im Inspector an.
/// </summary>
[ExecuteInEditMode]
public class ConditionalActionDebugger : MonoBehaviour
{
    [Header("Auto-Detect")]
    [SerializeField] bool autoDetect = true;

    [Header("Status")]
    [SerializeField] string statusMessage = "Click 'Validate Setup' button";

    [Header("References")]
    public ConditionalAction conditionalAction;
    public InteractTrigger interactTrigger;
    public DoorAction doorAction;
    public GameObject blockingCollider;

    void OnEnable()
    {
        if (autoDetect)
            AutoDetect();
    }

    void AutoDetect()
    {
        conditionalAction = GetComponent<ConditionalAction>();
        interactTrigger = GetComponent<InteractTrigger>();
        doorAction = GetComponent<DoorAction>();
    }

    public void ValidateSetup()
    {
        AutoDetect();
        
        string report = "=== DOOR SETUP VALIDATION ===\n\n";
        int errors = 0;
        int warnings = 0;

        // Check ConditionalAction
        if (conditionalAction == null)
        {
            report += "❌ ERROR: ConditionalAction component missing!\n";
            errors++;
        }
        else
        {
            report += "✅ ConditionalAction found\n";

            // Check Flag ID
            var flagIdField = conditionalAction.GetType().GetField("requiredFlagId", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (flagIdField != null)
            {
                string flagId = flagIdField.GetValue(conditionalAction) as string;
                if (string.IsNullOrEmpty(flagId))
                {
                    report += "⚠️ WARNING: Required Flag Id is empty\n";
                    warnings++;
                }
                else
                {
                    report += $"✅ Flag ID: '{flagId}'\n";
                }
            }

            // Check Action To Execute
            var actionField = conditionalAction.GetType().GetField("actionToExecute", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (actionField != null)
            {
                var action = actionField.GetValue(conditionalAction) as MonoBehaviour;
                if (action == null)
                {
                    report += "❌ ERROR: Action To Execute not set!\n";
                    errors++;
                }
                else
                {
                    report += $"✅ Action To Execute: {action.GetType().Name}\n";
                }
            }

            // Check Blocking Collider
            var blockingField = conditionalAction.GetType().GetField("blockingCollider", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (blockingField != null)
            {
                blockingCollider = blockingField.GetValue(conditionalAction) as GameObject;
                if (blockingCollider == null)
                {
                    report += "⚠️ WARNING: Blocking Collider not set (door won't block)\n";
                    warnings++;
                }
                else
                {
                    report += $"✅ Blocking Collider: {blockingCollider.name}\n";

                    // Check if child
                    if (blockingCollider.transform.parent != transform)
                    {
                        report += "⚠️ WARNING: Blocking Collider is not a child of this GameObject\n";
                        warnings++;
                    }

                    // Check collider settings
                    var col = blockingCollider.GetComponent<Collider>();
                    if (col == null)
                    {
                        report += "❌ ERROR: Blocking Collider has no Collider component!\n";
                        errors++;
                    }
                    else if (col.isTrigger)
                    {
                        report += "❌ ERROR: Blocking Collider 'Is Trigger' must be FALSE!\n";
                        errors++;
                    }
                    else
                    {
                        report += "✅ Blocking Collider setup correct\n";
                    }
                }
            }
        }

        // Check InteractTrigger
        if (interactTrigger == null)
        {
            report += "❌ ERROR: InteractTrigger component missing!\n";
            errors++;
        }
        else
        {
            report += "✅ InteractTrigger found\n";
        }

        // Check DoorAction
        if (doorAction == null)
        {
            report += "⚠️ WARNING: DoorAction component missing (if this is a door)\n";
            warnings++;
        }
        else
        {
            report += "✅ DoorAction found\n";
        }

        // Check own collider
        var ownCollider = GetComponent<Collider>();
        if (ownCollider == null)
        {
            report += "❌ ERROR: This GameObject needs a Collider (Is Trigger = TRUE)!\n";
            errors++;
        }
        else if (!ownCollider.isTrigger)
        {
            report += "⚠️ WARNING: This GameObject's Collider should have 'Is Trigger = TRUE'\n";
            warnings++;
        }
        else
        {
            report += "✅ Main collider setup correct\n";
        }

        // Summary
        report += $"\n=== SUMMARY ===\n";
        report += $"Errors: {errors}\n";
        report += $"Warnings: {warnings}\n";

        if (errors == 0 && warnings == 0)
        {
            report += "\n🎉 PERFECT SETUP! Ready to use!";
        }
        else if (errors == 0)
        {
            report += "\n✅ Setup is functional but has warnings";
        }
        else
        {
            report += "\n❌ Setup has errors - please fix!";
        }

        statusMessage = report;
        Debug.Log(report);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ConditionalActionDebugger))]
public class ConditionalActionDebuggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ConditionalActionDebugger debugger = (ConditionalActionDebugger)target;

        EditorGUILayout.Space();
        
        if (GUILayout.Button("Validate Setup", GUILayout.Height(40)))
        {
            debugger.ValidateSetup();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(debugger.GetType().GetField("statusMessage", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(debugger) as string, MessageType.Info);
    }
}
#endif