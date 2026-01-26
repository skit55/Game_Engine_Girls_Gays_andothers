using UnityEngine;

[ExecuteAlways]
public class StandardCurve : MonoBehaviour
{
    public Direction direction;

    public Transform A;
    public Transform Control;
    public Transform B;

    [Header("Debug")]
    [Range(8, 40)]
    public int resolution = 20;

    Vector3 Evaluate(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 ac = Vector3.Lerp(A.position, Control.position, t);
        Vector3 cb = Vector3.Lerp(Control.position, B.position, t);
        return Vector3.Lerp(ac, cb, t);
    }

    void OnDrawGizmos()
    {
        if (!A || !B || !Control) return;

        // Farbe je Richtung (nur Debug, Feel!)
        Gizmos.color = direction switch
        {
            Direction.Left => Color.red,
            Direction.Right => Color.green,
            Direction.Up => Color.cyan,
            Direction.Down => Color.yellow,
            _ => Color.white
        };

        Vector3 prev = Evaluate(0f);
        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 p = Evaluate(t);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }

        // Punkte anzeigen
        Gizmos.DrawSphere(A.position, 0.05f);
        Gizmos.DrawSphere(Control.position, 0.05f);
        Gizmos.DrawSphere(B.position, 0.05f);
    }
}
public enum Direction { Left, Right, Up, Down }
