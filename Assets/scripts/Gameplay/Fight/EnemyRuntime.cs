using System;
using UnityEngine;

public class EnemyRuntime : MonoBehaviour
{
    public event Action Died;
    public event Action<int, int> HpChanged;

    int maxHp;
    int hp;

    public int HP => hp;
    public int MaxHP => maxHp;

    public bool dead;
    public void InitFrom(EnemyData data)
    {
        // Minimal: HP bleibt inspector-gesteuert.
        // Optional später: EnemyData bekommt maxHp Feld.
        Debug.Log("INIIT FROMMM");
        maxHp = data.maxHealth;
        hp = data.maxHealth;
        HpChanged?.Invoke(hp, maxHp);
        dead = false;
    }

    public void DealDamage(int amount)
    {
        if (hp <= 0) return;

        hp -= Mathf.Abs(amount);
        if (hp < 0) hp = 0;

        HpChanged?.Invoke(hp, maxHp);

        if (hp == 0)
        {
            dead = true;
            Died?.Invoke();
            
        }
    }
}
