using System.Collections;
using UnityEngine;

public class CameraShakeSimple : MonoBehaviour
{
    [SerializeField] Transform target; // z.B. dein CameraRig / CameraRoot
    Coroutine routine;
    Vector3 startLocalPos;
    [SerializeField] float strenght;
    [SerializeField] float duration;
    [SerializeField] float frequency;


    void Awake()
    {
        if (!target) target = transform;
        startLocalPos = target.localPosition;
    }

    public void Shake(float strength = 0.2f, float duration = 0.2f, float frequency = 35f)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Routine(strength, duration, frequency));
    }

    IEnumerator Routine(float strength, float duration, float frequency)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // auch bei TimeScale punch stabil
            float damper = 1f - (t / duration); // abklingen

            // pseudo-random jitter
            float x = (Mathf.PerlinNoise(Time.time * frequency, 0f) * 2f - 1f);
            float y = (Mathf.PerlinNoise(0f, Time.time * frequency) * 2f - 1f);

            target.localPosition = startLocalPos + new Vector3(x, y, 0f) * strength * damper;
            yield return null;
        }

        target.localPosition = startLocalPos;
        routine = null;
    }
}
