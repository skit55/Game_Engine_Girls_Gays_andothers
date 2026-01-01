using System.Collections;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [Header("Disable these when NOT in Exploration")]
    [SerializeField] Behaviour[] disableOutsideExploration;

    void Start()
    {
        // Start läuft garantiert NACH allen Awakes
        StartCoroutine(BindWhenReady());
    }

    IEnumerator BindWhenReady()
    {
        // warten bis GSM existiert
        while (GameStateManager.Instance == null)
            yield return null;

        Debug.Log("PlayerBrain binding to GSM id=" + GameStateManager.Instance.GetInstanceID());

        GameStateManager.Instance.OnStateChanged -= Apply;
        GameStateManager.Instance.OnStateChanged += Apply;

        // initial anwenden
        Apply(GameStateManager.Instance.CurrentState);
    }

    void OnDisable()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged -= Apply;
    }

    void Apply(GameState state)
    {
        Debug.Log("PlayerBrain Apply: " + state);

        bool exploration = (state == GameState.Exploration);

        for (int i = 0; i < disableOutsideExploration.Length; i++)
        {
            if (disableOutsideExploration[i] != null)
                disableOutsideExploration[i].enabled = exploration;
        }
    }
}
