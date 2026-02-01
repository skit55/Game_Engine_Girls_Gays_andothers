using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] int damage = 1;
    [SerializeField] Transform destroyRoot;

    [Header("VFX")]
    [SerializeField] ParticleSystem parryVfx;   // im Prefab als Child zuweisen
    [SerializeField] ParticleSystem hitVfx;     // optional: wenn Player getroffen wird

    bool resolved;

    void Awake()
    {
        if (!destroyRoot) destroyRoot = transform.root;

        // Optional: falls nicht zugewiesen, versuch es automatisch im Root/Children zu finden
        // (nur wenn du willst)
        // if (!parryVfx) parryVfx = GetComponentInChildren<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (resolved) return;

        // --- Parry
        var zone = other.GetComponentInParent<ParryZone>();
        if (zone != null)
        {
            resolved = true;
            Debug.Log("Projectile was PARRIED");
            SfxManager.Instance.PlaySFX2D(SfxBank.Instance.parry);

            PlayAndDetach(parryVfx);
            Destroy(destroyRoot.gameObject);
            FindFirstObjectByType<CameraShakeSimple>()?.Shake();
            return;
        }

        // --- Player Hit
        var hurt = other.GetComponentInParent<PlayerHurtbox>();
        if (hurt != null)
        {
            resolved = true;
            Debug.Log("Projectile has HIT the player");
            SfxManager.Instance.PlaySFX2D(SfxBank.Instance.hurt);

            hurt.ApplyDamage(damage);
            PlayAndDetach(hitVfx);
            Destroy(destroyRoot.gameObject);
        }
    }

    void PlayAndDetach(ParticleSystem fx)
    {
        if (!fx) return;

        // vom Projectile lösen, damit Destroy(destroyRoot) es nicht mitkillt
        fx.transform.SetParent(null, true);

        // sicherstellen: einmalig, nicht looped
        var main = fx.main;
        main.loop = false;

        fx.Play();

        // Wenn im ParticleSystem "Stop Action = Destroy" gesetzt ist, brauchst du das nicht.
        // Das ist der fallback, falls nicht gesetzt:
        /*float lifetime = main.duration;

        // duration + maxStartLifetime (besser als nur duration)
        if (main.startLifetime.mode == ParticleSystemCurveMode.Constant)
            lifetime += main.startLifetime.constant;
        else
            lifetime += main.startLifetime.constantMax;

        Destroy(fx.gameObject, lifetime + 0.1f);*/
    }
}