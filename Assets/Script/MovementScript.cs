using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float rotationSpeed;
    public float moveAcceleration;
    public float maxMovementSpeed;
    public float jumpSpeed;
    public float fallAcceleration;
    public float friction;
    public CharacterController controller;
    public Transform rotatedTransform;
    public Transform cameraTransform;
    public Animator animator;
    public State state;

    private Vector3 moveDirection;
    private float verticalSpeed;

    void Start()
    {
        moveDirection = Vector3.zero;
    }

    void Update()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
        rotatedTransform.rotation = Quaternion.Lerp(rotatedTransform.rotation, Quaternion.LookRotation(cameraForward, Vector3.up), rotationSpeed * Time.deltaTime);

        Vector3 newMoveDirection = Vector3.zero;

        // TODO : Maybe should do jump in FixedUpdate?
        if (controller.isGrounded)
        {
            verticalSpeed = 0;
        }
        else
        {
            verticalSpeed -= fallAcceleration * Time.deltaTime;
        }

        if (state.idle)
        {
            // TODO : Project based on normal!
            Vector3 forwardMove = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            if (Input.GetKey(KeyCode.W))
            {
                newMoveDirection += forwardMove;
            }
            if (Input.GetKey(KeyCode.A))
            {
                // TODO : Rotate based on normal!
                newMoveDirection += Quaternion.AngleAxis(-90, Vector3.up) * forwardMove;
            }
            if (Input.GetKey(KeyCode.S))
            {
                newMoveDirection += -forwardMove;
            }
            if (Input.GetKey(KeyCode.D))
            {
                // TODO : Rotate based on normal!
                newMoveDirection += Quaternion.AngleAxis(90, Vector3.up) * forwardMove;
            }
            if (Input.GetKey(KeyCode.D))
            {
                // TODO : Rotate based on normal!
                newMoveDirection += Quaternion.AngleAxis(90, Vector3.up) * forwardMove;
            }
            moveDirection = newMoveDirection.normalized;
            if (controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    verticalSpeed = jumpSpeed;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            moveDirection = Vector3.zero;
            switch (state.attackPhase)
            {
                case 0:
                    animator.SetTrigger("attack1");
                    break;
                case 1:
                    animator.SetTrigger("attack2");
                    break;
                case 2:
                    animator.SetTrigger("attack3");
                    break;
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            moveDirection = Vector3.zero;
            animator.SetTrigger("guard");
        }
        animator.SetBool("walking", moveDirection != Vector3.zero);
    }

    void FixedUpdate()
    {
        // TODO : controller.velocity gets combined velocities, not only movement velocity. Need to separate and process separately!
        // TODO : friction should apply to the combined acceleration, not just move acceleration!
        Vector3 moveVelocity = controller.velocity + moveDirection * moveAcceleration;
        moveVelocity -= friction * controller.velocity;
        if (moveVelocity.magnitude > maxMovementSpeed)
        {
            moveVelocity = moveVelocity.normalized * maxMovementSpeed;
        }
        controller.Move((moveVelocity + new Vector3(0, verticalSpeed, 0)) * Time.fixedDeltaTime);
    }
}
