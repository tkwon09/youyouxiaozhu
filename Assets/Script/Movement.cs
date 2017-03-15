using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float walkingAcceleration = 0.1f;
    public float walkingDeceleration = -4f;
    public float turnAcceleration = 0.2f;
    public float walkingSpeedMax = 10f;
    float walkingSpeed = 0f;

    State playerState;

    Vector3 frameDirection = Vector3.zero;
    Vector3 faceDirection;
    Animator anim;
    Camera cam;
    Vector3 camdis;

	// Use this for initialization
	void Start ()
    {
        faceDirection = transform.eulerAngles;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        cam = Camera.main;
        camdis = cam.transform.position - transform.position;
        playerState = transform.GetChild(0).GetComponent<State>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playerState.idle)
        {
            if (Input.GetKeyDown(KeyCode.W))
                StartCoroutine(SpeedUp('W'));
            if (Input.GetKeyDown(KeyCode.S))
                StartCoroutine(SpeedUp('S'));
            if (Input.GetKeyDown(KeyCode.A))
                StartCoroutine(SpeedUp('A'));
            if (Input.GetKeyDown(KeyCode.D))
                StartCoroutine(SpeedUp('D'));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Decelerate();
            switch(playerState.attackPhase)
            {
                case 0:
                    anim.SetTrigger("attack1");
                    playerState.attackPhase = 1;
                    break;
                case 1:
                    anim.SetTrigger("attack2");
                    playerState.attackPhase = 2;
                    break;
                case 2:
                    anim.SetTrigger("attack3");
                    playerState.attackPhase = 0;
                    break;
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            Decelerate();
            anim.SetTrigger("guard");
        }

            if (Input.GetKeyUp(KeyCode.W))
                StartCoroutine(SpeedDown('W'));
            if (Input.GetKeyUp(KeyCode.S))
                StartCoroutine(SpeedDown('S'));
            if (Input.GetKeyUp(KeyCode.A))
                StartCoroutine(SpeedDown('A'));
            if (Input.GetKeyUp(KeyCode.D))
                StartCoroutine(SpeedDown('D'));
    }

    void FixedUpdate()
    {
        cam.transform.position = transform.position + camdis;

        if (frameDirection != Vector3.zero)
        {
            transform.eulerAngles = new Vector3(0f, -90 - Mathf.Atan2(frameDirection[2], (frameDirection[0] + 0.000001f)) * Mathf.Rad2Deg, 0f);
            transform.position += walkingSpeed * frameDirection.normalized * Time.fixedDeltaTime;
        }
    }

    void Accelerate()
    {
        if (playerState.walking)
            walkingSpeed = walkingSpeedMax;
        else
        {
            frameDirection = Vector3.zero;
            anim.SetBool("walking", false);
        }
    }

    void Decelerate()
    {
        playerState.walking = false;
        walkingSpeed = 0f;
        anim.SetBool("walking", false);
    }

    IEnumerator SpeedUp(char direction)
    {
        int positive = 0;
        int index = 0;

        switch (direction)
        {
            case 'W':
                positive = 1;
                index = 2;
                break;
            case 'S':
                positive = -1;
                index = 2;
                break;
            case 'A':
                positive = -1;
                index = 0;
                break;
            case 'D':
                positive = 1;
                index = 0;
                break;
        }
        anim.SetBool("walking", true);
        playerState.walking = true;
        Accelerate();
        while (frameDirection[index] != 1f && frameDirection[index] != -1f)
        {
            yield return new WaitForFixedUpdate();
            frameDirection[index] += positive * turnAcceleration;
            if (frameDirection[index] >= 1f || frameDirection[index] <= -1f)
                frameDirection[index] = frameDirection[index] > 0 ? 1f : -1f;
        }
        if (!playerState.walking)
        {
            frameDirection[index] = 0f;
            anim.SetBool("walking", false);
        }
    }

    IEnumerator SpeedDown(int direction)
    {
        int positive = 0;
        int index = 0;

        switch (direction)
        {
            case 'W':
                positive = 1;
                index = 2;
                break;
            case 'S':
                positive = -1;
                index = 2;
                break;
            case 'A':
                positive = -1;
                index = 0;
                break;
            case 'D':
                positive = 1;
                index = 0;
                break;
        }
        if (frameDirection[index == 0 ? 2 : 0] == 0f)
        {
            frameDirection[index] = 0f;
            Decelerate();
        }
        while (frameDirection[index] != 0f)
        {
            yield return new WaitForFixedUpdate();
            frameDirection[index] -= positive * turnAcceleration;
            if (positive > 0 && frameDirection[index] <= 0f || positive < 0 && frameDirection[index] >= 0f)
                frameDirection[index] = 0f;
        }
        if (frameDirection[index == 0 ? 2 : 0] == 0f)
        {
            frameDirection[index] = 0f;
            Decelerate();
        }

    }
}
