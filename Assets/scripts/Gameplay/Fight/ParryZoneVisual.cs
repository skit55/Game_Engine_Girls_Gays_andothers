using UnityEngine;

public class ParryZoneVisual : MonoBehaviour
{
    [SerializeField] GameObject onObject;

    void Awake()
    {
        // default: off
        if (onObject) onObject.SetActive(false);
    }

    public void SetArmed(bool armed)
    {
        if (onObject) onObject.SetActive(armed);
    }
}
