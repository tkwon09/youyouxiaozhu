using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : MonoBehaviour, EnemyBehaviors {

    Animator anim;
    EnemyAttributes attr;
    EnemyAttack attack;
    int scream;
    int basicAttack;
    int getHit;
    int walk;
    int die;
    int run;

    public int maxHealth;
    public int maxChi;
    public int IP;
    public float attackCooldown;
    public float attackRange;
    public float maxSpeed;

    public damageType basicDamageType;
    public int basicDamageP;
    public int basicDamageC;
    public damageType roarDamageType;
    public int roarDamageP;
    public int roarDamageC;
    public int roarCost;
    public float roarProb;
    public Element element;
    public int[] elementResistance = new int[5];

    damage basicDamage;
    damage roarDamage;

    void Awake()
    {
        anim = GetComponent<Animator>();
        attr = GetComponent<EnemyAttributes>();
        attack = attr.GetAttack();
        scream = Animator.StringToHash("Shout");
        basicAttack = Animator.StringToHash("Attack");
        getHit = Animator.StringToHash("GetHit");
        walk = Animator.StringToHash("Walk");
        die = Animator.StringToHash("Dead");
        run = Animator.StringToHash("Run");
        if (basicDamageC > 0)
            basicDamage = new damage(basicDamageType, basicDamageP, basicDamageC, element);
        else
            basicDamage = new damage(basicDamageType, basicDamageP, basicDamageC);
        roarDamage = new damage(roarDamageType, roarDamageP, roarDamageC, element);
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
    bool EnemyBehaviors.Special()
    {
        if (Random.value < roarProb)
        {
            attack.SetWholeCurrentDamage(roarDamage);
            Scream();
            return true;
        }
        return false;
    }
    int EnemyBehaviors.GetSpecialCost()
    {
        return roarCost;
    }
    void EnemyBehaviors.GetBehaviorParams(out float attackcooldown, out float attackrange, out float maxspeed)
    {
        attackcooldown = attackCooldown;
        attackrange = attackRange;
        maxspeed = maxSpeed;
    }
    void EnemyBehaviors.GetAttrbuteParams(out int maxhealth, out int maxchi, out int ip, int[] elementResis)
    {
        maxhealth = maxHealth;
        maxchi = maxChi;
        ip = IP;
        for (int i = 0; i < 5; i++)
            elementResis[i] = elementResistance[i];
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
