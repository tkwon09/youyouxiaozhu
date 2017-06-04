using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface EnemyBehaviors
{
    void GetHurt();
    void Attack();
    void Die();
    void Parry();
}

// Should have Animator parameters uniform with a blend tree of walk and run
public class EnemyBehavior : MonoBehaviour {

    public enum EnemyType { idle, attack, guard};
    public EnemyType type;
    public bool functioning;
    public float attackCooldown;
    public float attackRange = 1f;
    public float speed;

    float currspeed = 0;
    float acc;

    GameObject player;
    string etype;

    bool offencing;
    bool attackReady = true;
    bool isAttacking;

    Animator anim;
    int animSpeed;
    int animAttack;

    public EnemyBehaviors behavior;

	// Use this for initialization
	void Awake ()
    {
        etype = GetComponent<EnemyAttributes>().enemyType;
        behavior = (EnemyBehaviors)GetComponent(Type.GetType(etype));
        player = GameObject.FindGameObjectWithTag("Player");
        animSpeed = Animator.StringToHash("Speed");
        animAttack = Animator.StringToHash("Attack");
        anim = GetComponent<Animator>();
        acc = speed / 4;
        StartCoroutine(Behave());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        if(offencing)
        {
            Vector3 dest = (player.transform.position - transform.position);
            transform.rotation = Quaternion.LookRotation(dest);
            if (dest.magnitude > attackRange)
            {
                currspeed = Mathf.Clamp(currspeed + acc * Time.fixedDeltaTime, currspeed, speed);
                anim.SetFloat(animSpeed, currspeed / speed);
                transform.position += currspeed * dest.normalized * Time.fixedDeltaTime;
            }
            else
            {
                currspeed = 0;
                anim.SetFloat(animSpeed, currspeed / speed);
                if (attackReady)
                {
                    StartCoroutine(AttackCoolDown());
                    anim.SetTrigger(animAttack);
                }
            }
        }
    }


    IEnumerator Behave()
    {
        while(functioning)
        {
            yield return new WaitForSeconds(3);
            switch(type)
            {
                case EnemyType.attack:
                    offencing = true;
                    yield break;
                case EnemyType.guard:
                    behavior.Parry();
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator AttackCoolDown()
    {
        attackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }

    void StartAttack()
    {
        isAttacking = true;
    }
    void EndAttack()
    {
        isAttacking = false;
    }
}
