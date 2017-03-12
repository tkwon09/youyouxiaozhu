using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

    public bool idle = true;
    public bool walking = false;
    public int attackPhase = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void becomeBusy()
    {
        idle = false;
        walking = false;
    }
    void becomeFree()
    {
        idle = true;
        walking = false;
    }
    void attackPhase1()
    {
        attackPhase = 1;
    }
    void attackPhase2()
    {
        attackPhase = 2;
    }
    void attackPhase3()
    {
        attackPhase = 3;
    }
    void endAttack()
    {
        attackPhase = 0;
    }
}
