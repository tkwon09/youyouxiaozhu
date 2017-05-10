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
    public Attributes attr;
    public MovementScript move;
    public CombatControl cc;
    public int maxAttackPhase;
    public Transform rotationtransform;
    public GameObject swordChi;

    bool inCombo;
    int counter = 0;
    UIText uitext;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Initialize());
        uitext = GetComponent<UIText>();
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.5f);
        maxAttackPhase = attack.maxAttackPhase;
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
    void isAnimating()
    {
        attack.isAnimating = true;
        attackPhase++;
        uitext.UpdateUIText();
        attack.AddAttackPhaseBonus(attackPhase);
        StartCoroutine(ComboLast());
    }
    void endAnimating()
    {
        attack.isAnimating = false;
    }
    void isBlocking()
    {
        move.blocking = true;
    }
    void endBlocking()
    {
        move.blocking = false;
    }
    void runAttack()
    {
        runAttacked = true;
    }
    void CreateFrontCast()
    {
        attr.CreateFrontCastChi();
    }
    void CreateChi()
    {
        attr.TurnChiOn();
    }

    IEnumerator ComboLast()
    {
        counter++;
        inCombo = true;
        yield return new WaitForSeconds(1.5f);
        if (counter == 1)
        {
            inCombo = false;
            attackPhase = 0;
            attack.ResetDamage();
            uitext.UpdateUIText();
        }
        counter--;
    }

}
