using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GateAction : MonoBehaviour, IAction
{
    public string PromptText => "Open";

    [SerializeField] public CanvasGroup closedGroup;
    [SerializeField] public CanvasGroup openGroup;
    [Range(0f, 2f)]
    [SerializeField] public float pause;
    [Range(0.2f, 2f)]
    [SerializeField] public float speed;

    public void Execute(PlayerTriggerSensor player)
    {
        StartCoroutine(Fade(pause, speed));
        
    }

    IEnumerator Fade(float pause, float speed)
    {
        LeanTween.alphaCanvas(closedGroup, 0f, speed);
        yield return new WaitForSeconds(1f);
        LeanTween.alphaCanvas(openGroup, 1f, speed);

    }
}
