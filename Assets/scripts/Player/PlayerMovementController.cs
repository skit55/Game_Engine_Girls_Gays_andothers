using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Rotation")]
    public Transform visualRoot;
    public float turnSmoothTime = 0.08f;

    [Header("Gravity")]
    public float gravity = -20f;

    CharacterController cc;
    Vector3 velocityXZ;
    float yVel;
    float turnVel;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // debug timeScale toggle (optional)
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("timeScale=" + Time.timeScale);
            Time.timeScale = 1f;
        }

        // ---- GameState gate: movement only in exploration
        var gsm = GameStateManager.Instance;
        bool canMove = (gsm == null) || gsm.CanPlayerMove();

        // --- Input (WASD) only if allowed
        float x = canMove ? Input.GetAxisRaw("Horizontal") : 0f;
        float z = canMove ? Input.GetAxisRaw("Vertical") : 0f;

        Vector3 input = new Vector3(x, 0f, z);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // --- Target velocity
        Vector3 targetVel = input * maxSpeed;

        // --- Smooth accel/decel
        float rate = (input.sqrMagnitude > 0.0001f) ? acceleration : deceleration;
        velocityXZ = Vector3.MoveTowards(velocityXZ, targetVel, rate * Time.deltaTime);

        // --- Rotate towards movement direction
        Vector3 flatVel = new Vector3(velocityXZ.x, 0f, velocityXZ.z);
        if (flatVel.sqrMagnitude > 0.001f)
        {
            float targetYaw = Mathf.Atan2(flatVel.x, flatVel.z) * Mathf.Rad2Deg;
            float currentYaw = (visualRoot ? visualRoot.eulerAngles.y : transform.eulerAngles.y);

            float yaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref turnVel, turnSmoothTime);

            if (visualRoot) visualRoot.rotation = Quaternion.Euler(0f, yaw, 0f);
            else transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        // --- Gravity / grounding
        if (cc.isGrounded && yVel < 0f) yVel = -2f;
        yVel += gravity * Time.deltaTime;

        // --- Move ONCE per frame
        Vector3 motion = new Vector3(velocityXZ.x, yVel, velocityXZ.z);
        cc.Move(motion * Time.deltaTime);
    }
}
