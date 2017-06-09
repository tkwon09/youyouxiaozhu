using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour {

    public bool idle = true;
    public bool walking = false;
    EnemyAttack attack;
    EnemyAttributes attr;

    private void Start()
    {
        attr = GetComponent<EnemyAttributes>();
        attack = attr.GetAttack();
    }

    void becomeBusy()
    {
        idle = false;
    }
    void becomeFree()
    {
        idle = true;
        walking = false;
    }
    void isAttacking()
    {
        attack.isAttack = true;
        attack.HurtPlayer();
    }
    void endAttacking()
    {
        attack.isAttack = false;
    }

}
