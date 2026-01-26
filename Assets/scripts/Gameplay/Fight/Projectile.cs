using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 A, Control, B;

    public float travelTime = 0.6f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float t;

    Vector3 Evaluate(float tt)
    {
        tt = Mathf.Clamp01(tt);
        var ac = Vector3.Lerp(A, Control, tt);
        var cb = Vector3.Lerp(Control, B, tt);
        return Vector3.Lerp(ac, cb, tt);
    }

    void Update()
    {
        if (travelTime <= 0f) return;

        t += Time.deltaTime / travelTime;
        float u = Mathf.Clamp01(t);
        float e = Mathf.Clamp01(ease.Evaluate(u));

        var pos = Evaluate(e);
        transform.position = pos;

        // optional look forward
        var next = Evaluate(Mathf.Clamp01(e + 0.002f));
        var dir = next - pos;
        if (dir.sqrMagnitude > 1e-6f) transform.forward = dir.normalized;

        if (u >= 1f) Destroy(gameObject);
    }
}
