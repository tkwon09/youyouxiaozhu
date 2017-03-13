using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float rotationSpeed;
    public float moveAcceleration;
    public float airMoveAcceleration;
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

    private Vector3 surfaceNormal;
    private Vector3 moveVelocity;

    void Start()
    {
        moveDirection = Vector3.zero;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        surfaceNormal = Vector3.up;
        moveVelocity = Vector3.zero;
    }

    void Update()
    {
        
        // TODO : Maybe should do jump in FixedUpdate?
        if (controller.isGrounded)
        {
            verticalSpeed = 0;
        }
        else
        {
            verticalSpeed -= fallAcceleration * Time.deltaTime;
        }

        Vector3 newMoveDirection = Vector3.zero;
        if (state.idle)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
            rotatedTransform.rotation = Quaternion.Lerp(rotatedTransform.rotation, Quaternion.LookRotation(cameraForward, Vector3.up), rotationSpeed * Time.deltaTime);
            Vector3 forwardMove = Vector3.ProjectOnPlane(cameraTransform.forward, surfaceNormal).normalized;
            if (Input.GetKey(KeyCode.W))
            {
                newMoveDirection += forwardMove;
            }
            if (Input.GetKey(KeyCode.A))
            {
                newMoveDirection += Quaternion.AngleAxis(-90, surfaceNormal) * forwardMove;
            }
            if (Input.GetKey(KeyCode.S))
            {
                newMoveDirection += -forwardMove;
            }
            if (Input.GetKey(KeyCode.D))
            {
                newMoveDirection += Quaternion.AngleAxis(90, surfaceNormal) * forwardMove;
            }
            if (Input.GetKey(KeyCode.D))
            {
                newMoveDirection += Quaternion.AngleAxis(90, surfaceNormal) * forwardMove;
            }
            if (controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    verticalSpeed = jumpSpeed;
                }
            }
        }
        moveDirection = newMoveDirection.normalized;

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
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 0.5f))
        {
            surfaceNormal = hit.normal;
        }
        else
        {
            surfaceNormal = Vector3.up;
        }
        Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.red);
        Debug.DrawRay(transform.position, surfaceNormal, Color.yellow);

        Vector3 groundAcceleration = moveDirection * moveAcceleration;
        if (controller.isGrounded)
        {
            // TODO : Need to apply friction to total ground velocity, not just move velocity!
            groundAcceleration -= friction * moveVelocity;
        }
        else
        {
            groundAcceleration *= airMoveAcceleration;
        }
        moveVelocity += groundAcceleration;
        if (moveVelocity.magnitude > maxMovementSpeed)
        {
            moveVelocity = moveVelocity.normalized * maxMovementSpeed;
        }

        controller.Move((moveVelocity + new Vector3(0, verticalSpeed, 0)) * Time.fixedDeltaTime);
    }
}
