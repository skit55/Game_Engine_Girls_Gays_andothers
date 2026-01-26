using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] StandardCurve[] curves;

    public void Spawn(Direction dir, EnemyData enemy)
    {
        var curve = GetCurve(dir);
        if (curve == null) return;
        if (enemy.projectilePrefab == null) { Debug.LogError("Enemy projectilePrefab is null"); return; }

        Projectile p = Instantiate(enemy.projectilePrefab);

        // 2D jitter (XY), Z bleibt unangetastet
        Vector2 jA2 = Random.insideUnitCircle * enemy.startJitter;
        Vector2 jC2 = Random.insideUnitCircle * enemy.controlJitter;

        Vector3 A = curve.A.position;
        Vector3 C = curve.Control.position;
        Vector3 B = curve.B.position;

        A.x += jA2.x; A.y += jA2.y;
        C.x += jC2.x; C.y += jC2.y;

        // Z fix (falls du absolut sicher gehen willst)
        A.z = curve.A.position.z;
        C.z = curve.Control.position.z;
        B.z = curve.B.position.z;

        p.A = A;
        p.Control = C;
        p.B = B;

        p.travelTime = enemy.projectileTravelTime;
    }

    StandardCurve GetCurve(Direction d)
    {
        foreach (var c in curves)
            if (c.direction == d) return c;
        return null;
    }
}
