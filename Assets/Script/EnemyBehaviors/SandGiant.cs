using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SandGiant : MonoBehaviour,EnemyBehaviors {

    Animator anim;
    EnemyAttributes attr;
    EnemyAttack attack;
    int smash;
    int basicAttack1;
    int basicAttack2;
    int getHit;
    int walk;
    int die;
    int run;

    public damageType basicDamageType;
    public int basicDamageP1;
    public int basicDamageP2;
    public int basicDamageC;
    public float attack1Prob;
    public damageType smashDamageType;
    public int smashDamageP;
    public int smashDamageC;
    public damage basicDamage1;
    public damage basicDamage2;
    public damage smashDamage;
    public int smashCost;
    public float smashProb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        attr = GetComponent<EnemyAttributes>();
        attack = attr.GetAttack();
        smash = Animator.StringToHash("Smash");
        basicAttack1 = Animator.StringToHash("Attack1");
        basicAttack2 = Animator.StringToHash("Attack2");
        getHit = Animator.StringToHash("Get Hit");
        walk = Animator.StringToHash("Walk");
        die = Animator.StringToHash("Die");
        run = Animator.StringToHash("Run");
        basicDamage1 = new damage(basicDamageType, basicDamageP1, basicDamageC);
        basicDamage2 = new damage(basicDamageType, basicDamageP2, basicDamageC);
        smashDamage = new damage(smashDamageType, smashDamageP, smashDamageC);
    }

    void EnemyBehaviors.Attack()
    {
        if (Random.value <= attack1Prob)
        {
            attack.SetWholeCurrentDamage(basicDamage1);
            BasicAttack1();
        }
        else
        {
            attack.SetWholeCurrentDamage(basicDamage2);
            BasicAttack2();
        }
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
    bool EnemyBehaviors.Special()
    {
        if (Random.value <= smashProb)
        {
            attack.SetWholeCurrentDamage(smashDamage);
            Smash();
            return true;
        }
        return false;
    }
    int EnemyBehaviors.GetSpecialCost()
    {
        return smashCost;
    }
    public void Smash()
    {
        anim.SetTrigger(smash);
    }

    public void BasicAttack1()
    {
        anim.SetTrigger(basicAttack1);
    }
    public void BasicAttack2()
    {
        anim.SetTrigger(basicAttack2);
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
