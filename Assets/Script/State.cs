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
    int maxAttackPhase;

    bool inCombo;
    int counter = 0;

    public GameObject tuchi;

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
        attackPhase++;
        StartCoroutine(ComboLast());
    }
    void endAttacking()
    {
        attack.isAttack = false;
    }
    void isAnimating()
    {
        attack.isAnimating = true;
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
    void isTuing()
    {
        attack.SetCurrentDamage(Attack.damageType.chi);
    }
    void isBlending()
    {
        attack.SetCurrentDamage(Attack.damageType.blended);
    }
    void runAttack()
    {
        runAttacked = true;
    }
    void Tuchi()
    {
        Instantiate(tuchi, transform.position, Quaternion.identity);
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
        }
        counter--;
    }
}
