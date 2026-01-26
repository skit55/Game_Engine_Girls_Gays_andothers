using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int maxHp = 10;
    public int currentHp = 10;

    public event Action Died;
    public event Action<int, int> HpChanged; // current, max

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;               

        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        HpChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int amount)
    {

        Mathf.Clamp(currentHp -= amount,0,maxHp);
        Debug.Log("TOOK Damage : " + amount + "  CurrentHP = " + currentHp);
        HpChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Died?.Invoke(); Debug.Log("DIED");
        }
    }

    public void Heal(int amount)
    {
        currentHp = Mathf.Min(maxHp, currentHp + amount);
        HpChanged?.Invoke(currentHp, maxHp);
    }
}
