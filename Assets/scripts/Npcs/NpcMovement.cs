using UnityEngine;

public class GrandmaWander : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float turnSpeed = 5f;
    public float waitTimeAtTarget = 2f;

    public Transform[] waypoints; // <-- deine Koordinaten

    private Transform currentTarget;
    private float waitTimer;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        PickNewTarget();
    }

    void Update()
    {
        if (currentTarget == null || waypoints.Length == 0)
            return;

        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0f;

        // Ziel erreicht
        if (direction.magnitude < 0.3f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtTarget)
            {
                PickNewTarget();
                waitTimer = 0f;
            }
            return;
        }

        // Drehen
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        // Bewegen
        Vector3 move = transform.forward * moveSpeed;
        controller.SimpleMove(move);
    }

    void PickNewTarget()
    {
        if (waypoints.Length == 0)
            return;

        currentTarget = waypoints[Random.Range(0, waypoints.Length)];
    }
}
