using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class DragonBoar : MonoBehaviour, EnemyBehaviors
{
    Animator anim;
    EnemyAttributes attr;
    EnemyAttack attack;
    int scream;
    int basicAttack;
    int getHit;
    int walk;
    int die;
    int run;

    public damageType basicDamageType;
    public int basicDamageP;
    public int basicDamageC;
    public damageType roarDamageType;
    public int roarDamageP;
    public int roarDamageC;
    public int roarCost;
    public float roarProb;

    damage basicDamage;
    damage roarDamage;

    void Awake()
    {
        anim = GetComponent<Animator>();
        attr = GetComponent<EnemyAttributes>();
        attack = attr.GetAttack();
        scream = Animator.StringToHash("Scream");
        basicAttack = Animator.StringToHash("Attack");
        getHit = Animator.StringToHash("Get Hit");
        walk = Animator.StringToHash("Walk");
        die = Animator.StringToHash("Die");
        run = Animator.StringToHash("Run");
        basicDamage = new damage(basicDamageType, basicDamageP, basicDamageC);
        roarDamage = new damage(roarDamageType, roarDamageP, roarDamageC);
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
    public void Scream ()
	{
		anim.SetTrigger(scream);
	}

	public void BasicAttack ()
	{
		anim.SetTrigger(basicAttack);
	}

	public void GetHit ()
	{
		anim.SetTrigger(getHit);
	}

	public void Walk ()
	{
		anim.SetTrigger(walk);
	}

	public void DieAnim ()
	{
		anim.SetTrigger(die);
	}

	public void Run ()
	{
		anim.SetTrigger(run);
	}
		
}
