using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandGiant : MonoBehaviour,EnemyBehaviors {

    Animator anim;
    EnemyAttributes attr;
    EnemyAttack attack;
    int smash;
    int basicAttack;
    int getHit;
    int walk;
    int die;
    int run;

    public damageType basicDamageType;
    public int basicDamageP;
    public int basicDamageC;
    public damageType smashDamageType;
    public int smashDamageP;
    public int smashDamageC;
    public damage basicDamage;
    public damage smashDamage;

    void Awake()
    {
        anim = GetComponent<Animator>();
        attr = GetComponent<EnemyAttributes>();
        attack = attr.GetAttack();
        attack.SetWholeCurrentDamage(basicDamage);
        smash = Animator.StringToHash("Smash");
        basicAttack = Animator.StringToHash("Attack");
        getHit = Animator.StringToHash("Get Hit");
        walk = Animator.StringToHash("Walk");
        die = Animator.StringToHash("Die");
        run = Animator.StringToHash("Run");
        basicDamage = new damage(basicDamageType, basicDamageP, basicDamageC);
        smashDamage = new damage(smashDamageType, smashDamageP, smashDamageC);
    }

    void EnemyBehaviors.Attack()
    {
        attack.SetWholeCurrentDamage(basicDamage);
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
        attack.SetWholeCurrentDamage(smashDamage);
        Smash();
    }
    public void Smash()
    {
        anim.SetTrigger(smash);
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
