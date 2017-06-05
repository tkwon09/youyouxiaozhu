using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandGiant : MonoBehaviour,EnemyBehaviors {

    Animator anim;
    int scream;
    int basicAttack;
    int getHit;
    int walk;
    int die;
    int run;


    void Awake()
    {
        anim = GetComponent<Animator>();
        scream = Animator.StringToHash("Smash");
        basicAttack = Animator.StringToHash("Attack");
        getHit = Animator.StringToHash("Get Hit");
        walk = Animator.StringToHash("Walk");
        die = Animator.StringToHash("Die");
        run = Animator.StringToHash("Run");
    }

    void EnemyBehaviors.Attack()
    {
        BasicAttack();
    }
    void EnemyBehaviors.GetHurt()
    {
        GetHit();
    }
    void EnemyBehaviors.Die()
    {
        DieAnim();
    }
    void EnemyBehaviors.Parry()
    {
        return;
    }
    void EnemyBehaviors.Special()
    {
        Scream();
    }
    public void Scream()
    {
        anim.SetTrigger(scream);
    }

    public void BasicAttack()
    {
        anim.SetTrigger(basicAttack);
    }

    public void GetHit()
    {
        anim.SetTrigger(getHit);
    }

    public void Walk()
    {
        anim.SetTrigger(walk);
    }

    public void DieAnim()
    {
        anim.SetTrigger(die);
    }

    public void Run()
    {
        anim.SetTrigger(run);
    }
}
