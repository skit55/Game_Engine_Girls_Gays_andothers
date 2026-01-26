using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string id;

    [Header("Optional: set player facing to this yaw")]
    public bool applyRotation = true;

    [Tooltip("Wenn true, nimmt er die Rotation dieses SpawnPoints (meist nur Y).")]
    public bool useSpawnRotation = true;

    [Header("Optional: look at a target instead")]
    public Transform lookTarget;

    public Quaternion GetRotation(Vector3 playerPos)
    {
        if (!applyRotation) return Quaternion.identity;

        if (lookTarget != null)
        {
            Vector3 dir = (lookTarget.position - playerPos);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                return Quaternion.LookRotation(dir.normalized, Vector3.up);
        }

        if (useSpawnRotation)
        {
            // meist nur yaw
            Vector3 e = transform.rotation.eulerAngles;
            return Quaternion.Euler(0f, e.y, 0f);
        }

        return Quaternion.identity;
    }
}
