using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 fwd = Camera.main.transform.forward;
        fwd.y = 0;
        transform.rotation = Quaternion.LookRotation(fwd);
    }
}
