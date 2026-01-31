using UnityEngine;

public class GrandmaWander : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float turnSpeed = 5f;
    public float wanderRadius = 5f;
    public float waitTimeAtTarget = 2f;

    private Vector3 targetPosition;
    private float waitTimer;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        PickNewTarget();
    }

    void Update()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtTarget)
            {
                PickNewTarget();
                waitTimer = 0f;
            }
            return;
        }

        // Smooth drehen
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        // Bewegen (CharacterController blockt Wände)
        Vector3 move = transform.forward * moveSpeed;
        controller.SimpleMove(move);
    }

    void PickNewTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        targetPosition = new Vector3(
            transform.position.x + randomPoint.x,
            transform.position.y,
            transform.position.z + randomPoint.y
        );
    }
}
