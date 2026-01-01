using UnityEngine;

[CreateAssetMenu(menuName = "Game/Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string speakerName;

    [TextArea(2, 6)]
    public string[] lines;
}
