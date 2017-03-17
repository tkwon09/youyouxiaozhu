using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform follow;
    public MovementScript movement;
    public Camera camera;
    
    public float distance = 5.0f;
    public float zoomSpeed;
    public float cameraMoveSpeed;
    public float cameraAvoidSpeed;
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
        targetRotation = transform.rotation;
        targetPosition = transform.position;
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
        else
        {
            avoidanceStrength = 1.0f;
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
        // TODO : Change horizontal avoidance to prefer moving behind the player.
        Vector3 vectorToPosition = follow.position - transform.position;
        bool notHit = true;
        for (int i = 1; i < obstacleAvoidanceIterations && notHit; ++i)
        {
            Vector3 vectorToNewPosition = follow.position + movement.GetMoveVelocity() * i * Time.deltaTime - transform.position;
            
            Ray playerRay = new Ray(transform.position, vectorToNewPosition);
            RaycastHit rayHit;
            if (Physics.Raycast(playerRay, out rayHit, vectorToNewPosition.magnitude))
            {
                if (rayHit.collider.gameObject.GetComponent<WallScript>())
                {
                    // Check cross product between vectors. If normalized cross product - surfaceNormal = zero vector, then it's positive x movement.
                    Vector3 crossTest = Vector3.Cross(Vector3.ProjectOnPlane(vectorToPosition, movement.GetSurfaceNormal()), Vector3.ProjectOnPlane(vectorToNewPosition, movement.GetSurfaceNormal())).normalized;
                    float sign = (movement.GetSurfaceNormal() - crossTest == Vector3.zero) ? 1 : -1;
                    x += sign * movement.GetMoveVelocity().magnitude * cameraAvoidSpeed * Time.deltaTime * avoidanceStrength * (obstacleAvoidanceIterations - i) / (obstacleAvoidanceIterations - 1);
                    notHit = false;
                }
            }
            Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z), Vector3.ProjectOnPlane(vectorToNewPosition, Vector3.up), Color.cyan);
        }
        
        notHit = true;
        for (int i = 1; i < obstacleAvoidanceIterations && notHit; ++i)
        {
            Vector3 vectorToNewPosition = follow.position + movement.GetMoveVelocity() * i * Time.deltaTime - transform.position;

            Ray playerRay = new Ray(transform.position, vectorToNewPosition);
            RaycastHit rayHit;
            if (Physics.Raycast(playerRay, out rayHit, vectorToNewPosition.magnitude))
            {
                WallScript wallScript = rayHit.collider.gameObject.GetComponent<WallScript>();
                if (wallScript && wallScript.camDirection != WallScript.CameraDirection.Horizontal)
                {
                    // Either dodge the obstacle by moving up or down
                    float sign = (wallScript.camDirection == WallScript.CameraDirection.Up) ? 1 : -1;
                    y += sign * movement.GetMoveVelocity().magnitude * cameraAvoidSpeed * Time.deltaTime * avoidanceStrength * (obstacleAvoidanceIterations - i) / (obstacleAvoidanceIterations - 1);
                    notHit = false;
                }
            }
        }

        // 2. Let distance increase if player is moving away from camera
        if (movement.GetMoveVelocity().magnitude != 0)
        {
            //Vector3 velocityRelativeToCamera = Vector3.Project(movement.GetMoveVelocity(), vectorToPosition);
            if (camera.transform.up != movement.GetSurfaceNormal())
            {
                float sign = (Vector3.Dot(vectorToPosition, movement.GetSurfaceNormal()) > 0) ? 1 : -1;
                y += sign * cameraAvoidSpeed * Time.deltaTime;
            }
        }

        // 3. Let distance increase if player is moving away from camera
        if (movement.GetMoveVelocity().magnitude != 0)
        {
            Vector3 velocityRelativeToCamera = Vector3.Project(movement.GetMoveVelocity(), vectorToPosition);
            if (Vector3.Dot(vectorToPosition, velocityRelativeToCamera) > 0)
            {
                targetDistance += velocityRelativeToCamera.magnitude * Time.deltaTime;
            }
        }

        // 4. Adjust distance if line of sight is blocked
        bool hit = true;
        Vector3 zoomIncrement = vectorToPosition.normalized * zoomSpeed * Time.deltaTime;
        Vector3 zoomTotal = Vector3.zero;
        while (hit)
        {
            Ray zoomRay = new Ray(transform.position - vectorToPosition.normalized + zoomTotal, vectorToPosition + vectorToPosition.normalized);
            RaycastHit zoomRayHit;
            hit = false;
            if (Physics.Raycast(zoomRay, out zoomRayHit, vectorToPosition.magnitude))
            {
                if (zoomRayHit.collider.gameObject.GetComponent<WallScript>())
                {
                    hit = true;
                    zoomTotal += zoomIncrement;
                    targetDistance = vectorToPosition.magnitude - zoomTotal.magnitude;
                    if (targetDistance < distanceMin)
                    {
                        targetDistance = distanceMin;
                        hit = false;
                    }
                    distance = targetDistance;
                }
            }
        }

        // 2. Check if camera movement will block
        // 3. Check for camera collision
        // TODO : Decrease distance if there's something behind the camera or if the player's obstructed.
    }
}
