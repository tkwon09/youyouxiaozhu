using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour {

    public Animator animator;
    public Attack attack;
    public Attributes attr;
    public State state;

    int maxAttackPhase;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Initialize());
        
    }
	
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.5f);
        maxAttackPhase = attack.maxAttackPhase;
    }
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state.attackPhase == maxAttackPhase)
                return;
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    animator.SetTrigger("attack3");
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    animator.SetTrigger("attack2");
                }
                else
                {
                    animator.SetTrigger("attack1");
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("guard");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("fu");
        }
    }
}
