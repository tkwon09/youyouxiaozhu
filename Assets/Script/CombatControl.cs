using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour {

    public Animator animator;
    public Attack attack;
    public Attributes attr;
    public State state;
    

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Initialize());
        
    }
	
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.5f);
    }
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state.attackPhase == state.maxAttackPhase)
                return;
            
            switch (state.attackPhase)
            {
                case 0:
                    animator.SetTrigger("attack1");
                    break;
                case 1:
                    animator.SetTrigger("attack2");
                    break;
                case 2:
                    animator.SetTrigger("attack3");
                    break;
                default:
                    animator.SetTrigger("attack1");
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("guard");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!attr.chiOn && attr.StartChi())
            {
                animator.SetTrigger("fu");
            }
            else if(attr.chiOn)
            {
                attr.EndChi();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!attr.chiOn)
                return;
            animator.SetTrigger("frontcast");
        }
    }
}
