using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;

    void OnEnable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.HpChanged += OnHpChanged;

        // Initial refresh (falls Event schon vor UI kam)
        if (PlayerStats.Instance != null && healthText != null)
            healthText.text = PlayerStats.Instance.currentHp.ToString();
    }

    void OnDisable()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.HpChanged -= OnHpChanged;
    }

    void OnHpChanged(int current, int max)
    {
        current = Mathf.Max(0, current);
        Debug.Log("Display Reacts: " + current );

        if (healthText == null) return;
        healthText.text = current.ToString();
    }
}
