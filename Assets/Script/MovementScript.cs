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
    public float climbSpeed;
    public float fallAcceleration;
    public float floatAcceleration;
    public float maxFallSpeed;
    public float minRiseSpeed;
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
    public bool running;
    bool climbing = false;

    private Vector3 surfaceNormal;
    private Vector3 moveVelocity;
    private float currentMaxMoveSpeed;
    private float currentFriction;
    private float currentRotationSpeed;
    private float currentFallAcceleration;
    float climbAngle;


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
        currentFallAcceleration = fallAcceleration;
        currentFriction = airFriction;
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
                running = true;
                animator.SetBool("running", true);
            }
            if (!animator.GetBool("running"))
            {
            }
            if (moveDirection == Vector3.zero)
            {
                running = false;
                if (climbing)
                {
                    climbing = false;
                    rotatedTransform.rotation = Quaternion.Euler(0, rotatedTransform.rotation.eulerAngles.y, rotatedTransform.rotation.eulerAngles.z);
                }
                animator.SetBool("running", false);
            }
            else
            {
                if (climbing)
                {
                    Quaternion q = Quaternion.LookRotation(playerForward, Vector3.up);
                    rotatedTransform.rotation = Quaternion.Euler(climbAngle, q.eulerAngles.y,q.eulerAngles.z);
                }
                else
                    rotatedTransform.rotation = Quaternion.LookRotation(playerForward, Vector3.up);
                    // Quaternion.Lerp(rotatedTransform.rotation, Quaternion.LookRotation(playerForward, Vector3.up), currentRotationSpeed * Time.deltaTime);
                //Quaternion.Lerp(rotatedTransform.rotation, Quaternion.LookRotation(cameraForward, Vector3.up), currentRotationSpeed * Time.deltaTime);
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                currentMaxMoveSpeed = maxMovementSpeed;
                currentRotationSpeed = rotationSpeed;
                running = false;
                if (climbing)
                {
                    climbing = false;
                    rotatedTransform.rotation = Quaternion.Euler(0, rotatedTransform.rotation.eulerAngles.y, rotatedTransform.rotation.eulerAngles.z);
                }
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

        Vector3 moveAccelVector = moveDirection * ((onGround) ? moveAcceleration : airMoveAcceleration);

        // NOTE : This doesn't use onGround because want to let it keep falling.
        if (controller.isGrounded)
        {
            verticalSpeed = 0;
            currentFriction = friction;
        }
        else
        {
            verticalSpeed += -currentFallAcceleration * Time.fixedDeltaTime;
            currentFriction = airFriction;
        }
        if (verticalSpeed > 0)
        {
            if (verticalSpeed < minRiseSpeed)
            {
                currentFallAcceleration = floatAcceleration;
            }
        }
        else if (verticalSpeed < 0)
        {
            if (verticalSpeed < -minRiseSpeed)
            {
                currentFallAcceleration = fallAcceleration;
            }
        }
        if (verticalSpeed < -maxFallSpeed)
        {
            verticalSpeed = -maxFallSpeed;
        }

        moveVelocity += moveAccelVector * Time.fixedDeltaTime;
        moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, currentFriction * Time.fixedDeltaTime);

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
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(transform.position, hit.normal, Color.green);
        if (!running)
            return;
        WallScript wscript = hit.gameObject.GetComponent<WallScript>();
        if (wscript && wscript.camDirection == WallScript.CameraDirection.Horizontal)
        {
            verticalSpeed = climbSpeed;
            climbing = true;
            climbAngle = -Vector3.Angle(transform.up, hit.normal);
        }
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
