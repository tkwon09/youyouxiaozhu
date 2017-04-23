using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

    public bool idle = true;
    public bool walking = false;
    public int attackPhase = 0;
    public bool runAttacked = false;
    public Animator anim;
    public Attack attack;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void becomeBusy()
    {
        idle = false;
        //walking = false;
    }
    void becomeFree()
    {
        idle = true;
        walking = false;
        attackPhase = 0;
        anim.ResetTrigger("attack1");
        anim.ResetTrigger("attack2");
        anim.ResetTrigger("attack3");
        anim.ResetTrigger("guard");
    }
    void isAttacking()
    {
        attack.isAttack = true;
    }
    void endAttacking()
    {
        attack.isAttack = false;
    }
    void runAttack()
    {
        runAttacked = true;
    }
}
