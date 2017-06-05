using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

// Should have Walk and run Blend tree
// Step1: Copy read-only animation and complete animator
// Step2: Create attack boxes, attach EnemyAttack and tick "isTrigger"
// Step3: Add a moving collider and attach scripts(EnemyAttributes, EnemyBehavior, EnemyState)
// Step4: Add animation events
// Step5: Create corresponding class, implement interface and attach this class component
// Step6: Add GUI, Buffs and set "Enemy" tag
public class EnemyBehavior : MonoBehaviour {

    public EnemyType type;
    public bool functioning;
    public float attackCooldown;
    public float attackRange = 1f;
    public float speed;
    public float specialProb;
    public int specialCost;

    float currspeed = 0;
    float acc;

    GameObject player;
    string etype;

    bool offencing;
    bool attackReady = true;
    bool isAttacking;

    Animator anim;
    EnemyAttributes attr;
    int animSpeed;
    int animAttack;

    public EnemyBehaviors behavior;

    public bool isstun; // can't do anything
    public int stuncount;
    public bool issealed; // can't perform chi moves
    public int sealedcount;
    public bool istwined; // can't move or preform basic moves
    public int twinedcount;

    // Use this for initialization
    void Awake ()
    {
        etype = GetComponent<EnemyAttributes>().enemyType;
        attr = GetComponent<EnemyAttributes>();
        behavior = (EnemyBehaviors)GetComponent(Type.GetType(etype));
        player = GameObject.FindGameObjectWithTag("Player");
        animSpeed = Animator.StringToHash("Speed");
        anim = GetComponent<Animator>();
        acc = speed / 3;
        StartCoroutine(Behave());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        if (isstun)
            return;

        if(offencing)
        {
            Vector3 dest = (player.transform.position - transform.position);
            dest.Scale(new Vector3(1, 0, 1));
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
                    if (Random.value < specialProb && attr.Decrease(1, specialCost))
                        behavior.Special();
                    else
                        behavior.Attack();
                }
            }
        }
    }

    public void SetDisable(int index)
    {
        switch (index)
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
                if (stuncount == 0)
                    isstun = false;
                break;
            case 1:
                sealedcount--;
                if (sealedcount == 0)
                    issealed = false;
                break;
            case 2:
                twinedcount--;
                if (twinedcount == 0)
                    istwined = false;
                break;
            default:
                break;
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

    IEnumerator AttackCoolDown(int t = 0)
    {
        attackReady = false;
        if(t == 0)
            yield return new WaitForSeconds(attackCooldown);
        else
            yield return new WaitForSeconds(t);
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
