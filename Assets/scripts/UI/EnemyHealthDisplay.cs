using TMPro;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] EnemyRuntime enemy;
    [SerializeField] TextMeshProUGUI healthText;

    void OnEnable()
    {
        if (enemy != null)
            enemy.HpChanged += OnHpChanged;
        healthText.text = enemy.MaxHP.ToString();
    }

    void OnDisable()
    {
        if (enemy != null)
            enemy.HpChanged -= OnHpChanged;
    }

    void OnHpChanged(int current, int max)
    {
        current = Mathf.Max(0, current);
        Debug.Log("EnemyHealth: " + current);

        if (healthText == null) return;
        healthText.text = current.ToString();
    }

}
