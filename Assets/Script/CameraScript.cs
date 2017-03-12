using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float rotateSpeed;
    public Transform follow;
    
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMin = -20f;
    public float yMax = 80f;

    public float distanceMin = 2f;
    public float distanceMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        distance = distanceMax;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    // Update is called once per frame
    void Update ()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        if (y < -360F)
            y += 360F;
        if (y > 360F)
            y -= 360F;
        y = Mathf.Clamp(y, yMin, yMax);

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        transform.rotation = Quaternion.Euler(y, x, 0);
        transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -distance) + follow.position;
    }
}
