using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{

    public void ApplyDamage(int amount)
    {
        PlayerStats.Instance.TakeDamage(amount);
        FindObjectOfType<VignetteFlash>(true)?.Flash();

    }
}
