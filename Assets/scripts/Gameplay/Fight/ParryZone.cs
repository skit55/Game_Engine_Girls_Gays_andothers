using UnityEngine;

public class ParryZone : MonoBehaviour
{
    [SerializeField] Collider zoneCollider;
    [SerializeField] ParryZoneVisual visual;

    void Awake()
    {
        if (!zoneCollider) zoneCollider = GetComponent<Collider>();
        zoneCollider.isTrigger = true;
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        zoneCollider.enabled = active;
        if (visual) visual.SetArmed(active);
    }
}
