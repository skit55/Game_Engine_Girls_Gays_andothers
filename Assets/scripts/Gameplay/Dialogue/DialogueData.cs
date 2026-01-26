using UnityEngine;

[CreateAssetMenu(menuName = "Game/Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string speakerName;

    [TextArea(2, 6)]
    public string[] lines;

    [Header("Choices (optional, shown after last line)")]
    public bool hasChoices;

    [Tooltip("Texts for choice buttons (max 3)")]
    public string[] choiceTexts = new string[3];

    [Tooltip("Next dialogues for each choice (same index as text)")]
    public DialogueData[] choiceNext = new DialogueData[3];
}