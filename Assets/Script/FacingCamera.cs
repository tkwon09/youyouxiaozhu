using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour {

    Camera cam;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 fwd = cam.transform.forward;
        fwd.y = 0;
        transform.rotation = Quaternion.LookRotation(fwd);
    }
}
