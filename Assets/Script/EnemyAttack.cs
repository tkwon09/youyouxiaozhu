using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public bool isAttack;
    public bool isAnimating;
    bool withinRange;

    string attackTarget;
    public int maxAttackPhase;

    Animator anim;
    EnemyAttributes attr;
    GameObject player;
    public int damagePower;

    public List<wbuff> wbuffs = new List<wbuff>();
    public List<buff> buffs = new List<buff>();
    damage baseDamage;
    damage currentDamage;

    // Use this for initialization
    void Start()
    {
        attackTarget = "Player";
        attr = transform.parent.GetComponent<EnemyAttributes>();
        player = GameObject.FindGameObjectWithTag("Player");
        maxAttackPhase = attr.attrGet(4) / 10;
        baseDamage.type = damageType.physical;
        baseDamage.pDamage = damagePower * (1 + attr.attrGet(4) / 50);
        currentDamage = baseDamage;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddWbuff(buff weaponBuff)
    {
        buffs.Add(weaponBuff);
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag(attackTarget))
            withinRange = true;
        if (isAttack)
            HurtPlayer();
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.CompareTag(attackTarget))
            withinRange = false;
    }

    public void HurtPlayer()
    {
        if (!withinRange)
            return;
        Attributes a = player.GetComponent<Attributes>();
        a.TakeDamage(currentDamage);
        if (wbuffs.Count != 0)
        {
            foreach (wbuff item in wbuffs)
            {
                if (Random.value <= item.probability)
                    buffs.Add(item.buffToAdd);
            }
        }

        if (buffs.Count != 0)
        {
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                a.AddBuff(buffs[i]);
                buffs.RemoveAt(i);
            }
        }
    }


    public void SetWholeCurrentDamage(damage d)
    {
        currentDamage = d;
    }

    public void SetWholeCurrentDamage(damageType dt, int pd, int cd = 0)
    {
        currentDamage.type = dt;
        currentDamage.pDamage = pd;
        currentDamage.cDamage = cd;
    }

    public void SetCurrentDamage(int d, int a)
    {
        if (d != 0 && d != 1)
            return;
        if ((d == 1) && currentDamage.type == damageType.physical)
            currentDamage.type = damageType.blended;
        if (d == 0)
            currentDamage.pDamage = a;
        else
            currentDamage.cDamage = a;
    }

    public void AddCurrentDamage(int d, int a)
    {
        if (d != 0 && d != 1)
            return;
        if ((d == 1) && currentDamage.type == damageType.physical)
            currentDamage.type = damageType.blended;
        if (d == 0)
            currentDamage.pDamage += a;
        else
            currentDamage.cDamage += a;
    }

    public void AddAttackPhaseBonus(int phase)
    {
        int bonusDamage = (int)(currentDamage.pDamage * (1 + phase / 10.0f));
        SetCurrentDamage(0, bonusDamage);
    }

    public void ResetDamage(int d = 0)
    {
        if (d != 0 && d != 1)
            return;
        if (currentDamage.type == damageType.blended)
        {
            if (d == 0)
                currentDamage.type = damageType.chi;
            else
                currentDamage.type = damageType.physical;
        }
        if (d == 0)
            currentDamage.pDamage = damagePower * (1 + attr.attrGet(4) / 50);
        else
            currentDamage.cDamage = 0;
    }

    public void ResetWholeDamage()
    {
        currentDamage = baseDamage;
    }

    public string GetTargetLabel()
    {
        return attackTarget;
    }
}
