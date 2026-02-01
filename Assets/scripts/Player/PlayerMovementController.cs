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

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] string speedParam = "Speed";
    [SerializeField, Range(0f, 1f)] float speedDampTime = 0.08f; // smoothing im Animator

    CharacterController cc;
    Vector3 velocityXZ;
    float yVel;
    float turnVel;

    int speedHash;

    void Awake()
    {
        cc = GetComponent<CharacterController>();

        if (!animator)
            animator = GetComponentInChildren<Animator>();

        speedHash = Animator.StringToHash(speedParam);
    }

    void Update()
    {
        var gsm = GameStateManager.Instance;
        bool canMove = (gsm == null) || gsm.CanPlayerMove();

        float x = canMove ? Input.GetAxisRaw("Horizontal") : 0f;
        float z = canMove ? Input.GetAxisRaw("Vertical") : 0f;

        Vector3 input = new Vector3(x, 0f, z);
        if (input.sqrMagnitude > 1f) input.Normalize();

        Vector3 targetVel = input * maxSpeed;

        float rate = (input.sqrMagnitude > 0.0001f) ? acceleration : deceleration;
        velocityXZ = Vector3.MoveTowards(velocityXZ, targetVel, rate * Time.deltaTime);

        Vector3 flatVel = new Vector3(velocityXZ.x, 0f, velocityXZ.z);
        if (flatVel.sqrMagnitude > 0.001f)
        {
            float targetYaw = Mathf.Atan2(flatVel.x, flatVel.z) * Mathf.Rad2Deg;
            float currentYaw = (visualRoot ? visualRoot.eulerAngles.y : transform.eulerAngles.y);

            float yaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref turnVel, turnSmoothTime);

            if (visualRoot) visualRoot.rotation = Quaternion.Euler(0f, yaw, 0f);
            else transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        if (cc.isGrounded && yVel < 0f) yVel = -2f;
        yVel += gravity * Time.deltaTime;

        Vector3 motion = new Vector3(velocityXZ.x, yVel, velocityXZ.z);
        cc.Move(motion * Time.deltaTime);

        // ---- Animation: Speed (0..1)
        if (animator)
        {
            float speed01 = Mathf.Clamp01(flatVel.magnitude / Mathf.Max(0.0001f, maxSpeed));
            animator.SetFloat(speedHash, speed01, speedDampTime, Time.deltaTime);
        }
    }
}
