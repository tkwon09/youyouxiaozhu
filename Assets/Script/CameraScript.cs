using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform follow;
    public MovementScript movement;
    
    public float distance = 5.0f;
    public float zoomSpeed;
    public float cameraMoveSpeed;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMin = -20f;
    public float yMax = 80f;

    public float distanceMin = 2f;
    public float distanceMax = 15f;

    public int obstacleAvoidanceIterations;
    public float avoidanceRecoveryTime;

    float x = 0.0f;
    float y = 0.0f;
    private float targetDistance;
    private Quaternion targetRotation;
    private Vector3 targetPosition;
    float avoidanceStrength;

    // Use this for initialization
    void Start()
    {
        distance = distanceMax;
        targetDistance = distance;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        avoidanceStrength = 1;
    }

    // Update is called once per frame
    void Update ()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            avoidanceStrength = 0;
        }
        else if (avoidanceStrength < 1.0f)
        {
            avoidanceStrength += Time.deltaTime / avoidanceRecoveryTime;
        }

        if (y < -360F)
            y += 360F;
        if (y > 360F)
            y -= 360F;
        y = Mathf.Clamp(y, yMin, yMax);

        targetDistance = Mathf.Clamp(targetDistance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
        distance = Mathf.Lerp(distance, targetDistance, zoomSpeed * Time.deltaTime);

        targetRotation = Quaternion.Euler(y, x, 0);
        targetPosition = targetRotation * new Vector3(0.0f, 0.0f, -distance) + follow.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraMoveSpeed * 1.2f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraMoveSpeed * Time.deltaTime);

        // 1. Check if player movement will block

        // TODO : Check multiple interations velocity iterations.
        bool notHit = true;
        for (int i = 1; i < obstacleAvoidanceIterations && notHit; ++i)
        {
            Vector3 vectorToPosition = follow.position - transform.position;
            Vector3 vectorToNewPosition = follow.position + movement.GetMoveVelocity() * i * Time.deltaTime - transform.position;
            
            Ray playerRay = new Ray(transform.position, vectorToNewPosition);
            RaycastHit rayHit;
            if (Physics.Raycast(playerRay, out rayHit, vectorToNewPosition.magnitude))
            {
                if (rayHit.collider.tag == "Wall")
                {
                    // Check cross product between vectors. If normalized cross product - surfaceNormal = zero vector, then it's positive x movement.
                    Vector3 crossTest = Vector3.Cross(Vector3.ProjectOnPlane(vectorToPosition, movement.GetSurfaceNormal()), Vector3.ProjectOnPlane(vectorToNewPosition, movement.GetSurfaceNormal())).normalized;
                    float sign = (movement.GetSurfaceNormal() - crossTest == Vector3.zero) ? 1 : -1;
                    x += sign * movement.GetMoveVelocity().magnitude * cameraMoveSpeed * Time.deltaTime * (obstacleAvoidanceIterations - i) / (obstacleAvoidanceIterations - 1) * avoidanceStrength;
                    notHit = false;
                }
            }
            Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z), Vector3.ProjectOnPlane(vectorToNewPosition, Vector3.up), Color.cyan);
        }

        // 2. Check if camera movement will block
        // 3. Check for camera collision
        // TODO : Decrease distance if there's something behind the camera or if the player's obstructed.
    }
}
