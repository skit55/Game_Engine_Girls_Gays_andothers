using UnityEngine;

public class DefeatableEncounter : MonoBehaviour
{
    [SerializeField] string encounterId = "StreetEnemy1";
    [SerializeField] GameObject disableRoot; // optional, sonst this.gameObject

    void Awake()
    {
        if (!disableRoot) disableRoot = gameObject;
    }

    void OnEnable()
    {
        if (WorldFlags.Instance != null && WorldFlags.Instance.IsDefeated(encounterId))
            disableRoot.SetActive(false);
    }
}
