using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControl : MonoBehaviour {

    public Animator animator;
    public Attack attack;
    public Attributes attr;
    public ChiBarDisplay TaiChi;
    public State state;
    public CameraScript cams;

    public bool isstun; // can't do anything
    public int stuncount;
    public bool issealed; // can't perform chi moves
    public int sealedcount;
    public bool istwined; // can't move or preform basic moves
    public int twinedcount;

	// Use this for initialization
	void Start ()
    {
        
    }
	
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.5f);
    }
	// Update is called once per frame
	void Update ()
    {
        if (isstun)
            return;

        if (!istwined)
        {
            #region Basic moves
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
                animator.SetTrigger("parry");
            }
            #endregion
        }

        if (issealed)
            return;

        #region Chi
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
            if (!attr.chiOn || !attr.UseChiSpell(0))
                return;
            animator.SetTrigger("frontcast");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!attr.chiOn || !attr.UseChiSpell(1) || cams.GetAimingTarget() == null)
                return;
            state.twineTarget = cams.GetAimingTarget();
            animator.SetTrigger("twinespell");
        }
        #endregion

        if(Input.GetKeyDown(KeyCode.Q))
        {
            TaiChi.ShowElement();
        }
        if(Input.GetKeyUp(KeyCode.Q))
        {
            TaiChi.HideElement();
        }
    }

    public void SetDisable(int index)
    {
        switch(index)
        {
            case 0:
                isstun = true;
                stuncount++;
                break;
            case 1:
                issealed = true;
                sealedcount++;
                break;
            case 2:
                istwined = true;
                twinedcount++;
                break;
            default:
                break;
        }
    }

    public void ResetDisable(int index)
    {
        switch (index)
        {
            case 0:
                stuncount--;
                if(stuncount == 0) 
                    isstun = false;
                break;
            case 1:
                sealedcount--;
                if(sealedcount == 0)
                    issealed = false;
                break;
            case 2:
                twinedcount--;
                if(twinedcount == 0)
                    istwined = false;
                break;
            default:
                break;
        }
    }
}
