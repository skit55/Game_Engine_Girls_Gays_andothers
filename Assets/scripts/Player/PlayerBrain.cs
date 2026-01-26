using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [Header("Enable these only in Exploration")]
    [SerializeField] Behaviour[] enabledInExploration;

    [Header("Enable these only in Fight")]
    [SerializeField] Behaviour[] enabledInFight;

    [SerializeField] public GameObject normalVisual;
    [SerializeField] public GameObject FightVisual;
    bool bound;

    void OnEnable() => TryBind();

    void Update()
    {
        if (!bound) TryBind();
    }

    void TryBind()
    {
        var gsm = GameStateManager.Instance;
        if (gsm == null) return;

        gsm.OnStateChanged -= Apply;
        gsm.OnStateChanged += Apply;

        Apply(gsm.CurrentState);
        bound = true;
    }

    void OnDisable()
    {
        var gsm = GameStateManager.Instance;
        if (gsm != null) gsm.OnStateChanged -= Apply;
        bound = false;
    }

    void Apply(GameState state)
    {
        bool exploration = state == GameState.Exploration;
        bool fight = state == GameState.Fight;

        // Exploration set
        for (int i = 0; i < enabledInExploration.Length; i++)
        {
            var b = enabledInExploration[i];
            if (b != null) b.enabled = exploration;
        }

        // Fight set
        for (int i = 0; i < enabledInFight.Length; i++)
        {
            var b = enabledInFight[i];
            if (b != null) b.enabled = fight;
            
        }

        normalVisual.SetActive(exploration);
        FightVisual.SetActive(!exploration);
    }
}
