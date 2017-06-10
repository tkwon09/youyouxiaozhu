using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public bool isAttack;
    public bool isAnimating;
    bool withinRange;

    string attackTarget;

    Animator anim;
    EnemyAttributes attr;
    GameObject player;

    public List<wbuff> wbuffs = new List<wbuff>();
    public List<buff> buffs = new List<buff>();
    damage currentDamage;

    // Use this for initialization
    void Start()
    {
        attackTarget = "Player";
        attr = transform.parent.GetComponent<EnemyAttributes>();
        player = GameObject.FindGameObjectWithTag("Player");
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

    public void SetWholeCurrentDamage(damageType dt, int pd, int cd = 0, Element e = Element.none)
    {
        currentDamage.type = dt;
        currentDamage.pDamage = pd;
        currentDamage.cDamage = cd;
        currentDamage.element = e;
    }

    public void SetCurrentDamage(int index, int amount, Element e = Element.none)
    {
        if (index != 0 && index != 1 || amount < 0)
            return;
        if (index == 1)
        {
            if (currentDamage.type == damageType.physical && amount > 0)
            {
                currentDamage.type = damageType.blended;
                currentDamage.element = e;
            }
            else if (currentDamage.type == damageType.blended && amount == 0)
            {
                currentDamage.type = damageType.physical;
                currentDamage.element = Element.none;
            }
        }
        if (index == 0)
            currentDamage.pDamage = amount;
        else
            currentDamage.cDamage = amount;
    }

    public void AddCurrentDamage(int index, int amount, Element e = Element.none)
    {
        if (index != 0 && index != 1)
            return;
        if (index == 1)
        {
            if (currentDamage.type == damageType.physical && amount > 0)
            {
                currentDamage.type = damageType.blended;
                if (e != Element.none && currentDamage.element != e)
                    if (currentDamage.cDamage <= amount)
                        currentDamage.element = e;

            }
            else if (currentDamage.type == damageType.blended && currentDamage.cDamage + amount <= 0)
            {
                currentDamage.type = damageType.physical;
                currentDamage.element = Element.none;
            }
        }

        if (index == 0)
            currentDamage.pDamage = Mathf.Clamp(currentDamage.pDamage + amount, 0, int.MaxValue);
        else
            currentDamage.cDamage = Mathf.Clamp(currentDamage.cDamage + amount, 0, int.MaxValue);
    }

    public void AddAttackPhaseBonus(int phase)
    {
        int bonusDamage = (int)(currentDamage.pDamage * (1 + phase / 10.0f));
        SetCurrentDamage(0, bonusDamage);
    }

    public void ResetDamage(int d = 0)
    {

    }

    public string GetTargetLabel()
    {
        return attackTarget;
    }
}
