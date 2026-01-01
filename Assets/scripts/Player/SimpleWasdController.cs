using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SmoothWasdController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float acceleration = 20f;   // wie schnell auf maxSpeed
    public float deceleration = 25f;   // wie schnell wieder stoppen

    [Header("Rotation")]
    public Transform visualRoot;       // z.B. dein "Visuals" oder CatMesh; wenn leer -> rotiert Player
    public float turnSmoothTime = 0.08f;

    [Header("Gravity")]
    public float gravity = -20f;

    CharacterController cc;
    Vector3 velocityXZ;               // smoothed horizontale Geschwindigkeit
    float yVel;
    float turnVel;                    // für SmoothDampAngle


    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- Input (WASD) inkl. Diagonal
        float x = Input.GetAxisRaw("Horizontal"); // A/D
        float z = Input.GetAxisRaw("Vertical") ;   // W/S

        Vector3 input = new Vector3(x, 0f, z);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // --- Zielgeschwindigkeit
        Vector3 targetVel = input * maxSpeed;

        // --- Smoothing: accel wenn Input, sonst decel
        float rate = (input.sqrMagnitude > 0.0001f) ? acceleration : deceleration;
        velocityXZ = Vector3.MoveTowards(velocityXZ, targetVel, rate * Time.deltaTime);

        // --- Rotation in Bewegungsrichtung (nur wenn wir uns wirklich bewegen)
        Vector3 flatVel = new Vector3(velocityXZ.x, 0f, velocityXZ.z);
        if (flatVel.sqrMagnitude > 0.001f)
        {
            float targetYaw = Mathf.Atan2(flatVel.x, flatVel.z) * Mathf.Rad2Deg;
            float currentYaw = (visualRoot ? visualRoot.eulerAngles.y : transform.eulerAngles.y);

            float yaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref turnVel, turnSmoothTime);

            if (visualRoot) visualRoot.rotation = Quaternion.Euler(0f, yaw, 0f);
            else transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        // --- Gravity (CharacterController braucht das für sauberes Grounding)
        if (cc.isGrounded && yVel < 0f) yVel = -2f;
        yVel += gravity * Time.deltaTime;

        // --- Move
        Vector3 motion = new Vector3(velocityXZ.x, yVel, velocityXZ.z);
        cc.Move(motion * Time.deltaTime);
    }
}