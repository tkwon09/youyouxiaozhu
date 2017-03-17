using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float rotationSpeed;
    public float runRotationSpeed;
    public float moveAcceleration;
    public float airMoveAcceleration;
    public float maxMovementSpeed;
    public float maxRunSpeed;
    public float dashSpeed;
    public float jumpSpeed;
    public float fallAcceleration;
    public float friction;
    public float airFriction;
    public CharacterController controller;
    public Transform rotatedTransform;
    public Transform cameraTransform;
    public Animator animator;
    public State state;

    private Vector3 moveDirection;
    private float verticalSpeed;
    private bool jumped;
    private bool onGround;

    private Vector3 surfaceNormal;
    private Vector3 moveVelocity;
    private float currentMaxMoveSpeed;
    private float currentRotationSpeed;

    void Start()
    {
        moveDirection = Vector3.zero;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        surfaceNormal = Vector3.up;
        moveVelocity = Vector3.zero;
        currentMaxMoveSpeed = maxMovementSpeed;
        currentRotationSpeed = rotationSpeed;
        jumped = false;
        onGround = false;
    }

    void Update()
    {
        Vector3 newMoveDirection = Vector3.zero;
        if (state.idle)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
            Vector3 playerForward = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
            Vector3 forwardMove = Vector3.ProjectOnPlane(cameraForward, surfaceNormal).normalized;
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentMaxMoveSpeed = maxRunSpeed;
                currentRotationSpeed = runRotationSpeed;
                animator.SetBool("running", true);
            }
            if (!animator.GetBool("running"))
            {
            }
            if (moveDirection == Vector3.zero)
            {
                animator.SetBool("running", false);
            }
            else
            {
                rotatedTransform.rotation = Quaternion.LookRotation(playerForward, Vector3.up);
                    // Quaternion.Lerp(rotatedTransform.rotation, Quaternion.LookRotation(playerForward, Vector3.up), currentRotationSpeed * Time.deltaTime);
                //Quaternion.Lerp(rotatedTransform.rotation, Quaternion.LookRotation(cameraForward, Vector3.up), currentRotationSpeed * Time.deltaTime);
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                currentMaxMoveSpeed = maxMovementSpeed;
                currentRotationSpeed = rotationSpeed;
                animator.SetBool("running", false);
            }
            if (onGround)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumped = true;
                }
            }
        }
        moveDirection = newMoveDirection.normalized;
        animator.SetBool("walking", moveDirection != Vector3.zero);

        if (Input.GetMouseButtonDown(0))
        {
            moveDirection = Vector3.zero;
            switch (state.attackPhase)
            {
                case 0:
                    animator.SetTrigger("attack1");
                    state.attackPhase = 1;
                    break;
                case 1:
                    animator.SetTrigger("attack2");
                    state.attackPhase = 2;
                    break;
                case 2:
                    animator.SetTrigger("attack3");
                    state.attackPhase = 0;
                    break;
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            moveDirection = Vector3.zero;
            animator.SetTrigger("guard");
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        onGround = Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, 0.5f);
        if (onGround)
        {
            surfaceNormal = hit.normal;
        }
        else
        {
            surfaceNormal = Vector3.up;
        }
        Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.red);
        Debug.DrawRay(transform.position, surfaceNormal, Color.yellow);

        Vector3 moveAccelVector = moveDirection * ((controller.isGrounded) ? moveAcceleration : airMoveAcceleration);
        //Vector3 frictionVector = moveVelocity * ((controller.isGrounded) ? friction : airFriction);

        // NOTE : This doesn't use onGround because want to let it keep falling.
        verticalSpeed += (controller.isGrounded) ? 0 : -fallAcceleration * Time.deltaTime;

        moveVelocity += moveAccelVector;
        moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, ((controller.isGrounded)? friction : airFriction) * Time.fixedDeltaTime);
        //moveVelocity -= frictionVector;
        if (state.idle && moveVelocity.magnitude > currentMaxMoveSpeed)
        {
            moveVelocity = moveVelocity.normalized * currentMaxMoveSpeed;
        }

        if (jumped)
        {
            verticalSpeed = jumpSpeed;
            jumped = false;
        }

        if (state.runAttacked)
        {
            moveVelocity = moveVelocity.normalized * dashSpeed;
            state.runAttacked = false;
        }

        //agent.Move((moveVelocity + new Vector3(0, verticalSpeed, 0)) * Time.fixedDeltaTime);
        controller.Move((moveVelocity + new Vector3(0, verticalSpeed, 0)) * Time.fixedDeltaTime);
    }

    public Vector3 GetMoveVelocity()
    {
        return moveVelocity;
    }

    public Vector3 GetSurfaceNormal()
    {
        return surfaceNormal;
    }
}
