using UnityEngine;

public class CameraFollowXOnly : MonoBehaviour
{
    public Transform target;          // Player hier reinziehen
    public float xOffset = 0f;        // falls Kamera etwas vor/nach links versetzt sein soll
    public bool smooth = true;
    public float smoothSpeed = 10f;

    float fixedY;
    float fixedZ;
    Quaternion fixedRot;

    void Start()
    {
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
        fixedRot = transform.rotation;   // Rotation einfrieren
    }

    void LateUpdate()
    {
        if (!target) return;

        float desiredX = target.position.x + xOffset;

        Vector3 desiredPos = new Vector3(desiredX, fixedY, fixedZ);

        if (smooth)
            transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        else
            transform.position = desiredPos;

        transform.rotation = fixedRot; // keine Rotation
    }
}
