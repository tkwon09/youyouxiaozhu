using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : MonoBehaviour
{

    bool isFriendly;
    public bool isAttack;
    public bool isAnimating;
    public bool getBlocked;

    string attackTarget;
    public List<GameObject> hitTargets = new List<GameObject>();
    public int maxAttackPhase;

    Animator anim;
    Attributes attr;
    GameObject player;
    public int weaponDamage;


    public float attackPhaseBonus = 0.2f;

    public struct buff
    {
        public string buffName;
        public bool isTemp;
        public float time;

        public buff(string bName, bool temp, float t)
        {
            buffName = bName;
            isTemp = temp;
            time = t;
        }
    }

    public struct wbuff
    {
        public buff buffToAdd;
        public float probability;

        public wbuff(buff b, float p)
        {
            buffToAdd = b;
            probability = p;
        }
    }

    public enum damageType { physical, blended, chi};
    public struct damage
    {
        public damageType type;
        public int pDamage;
        public int cDamage;

        public damage(damageType t, int pd, int cd = 0)
        {
            type = t;
            pDamage = pd;
            cDamage = cd;
        }
    }

    public List<wbuff> wbuffs = new List<wbuff>();
    public List<buff> buffs = new List<buff>();
    damage baseDamage;
    damage currentDamage;

    // Use this for initialization
    void Start ()
    {
        if (CompareTag("Enemy"))
            isFriendly = false;
        else
            isFriendly = true;
        if (isFriendly)
            attackTarget = "Enemy";
        else
            attackTarget = "Player";
        anim = transform.parent.parent.parent.GetComponent<Animator>();
        attr = transform.parent.parent.parent.parent.parent.GetComponent<Attributes>();
        player = transform.parent.parent.parent.parent.parent.gameObject;
        maxAttackPhase = attr.attrGet(4)/10;
        baseDamage.type = damageType.physical;
        baseDamage.pDamage = weaponDamage * (1 + attr.attrGet(4) / 50);
        currentDamage = baseDamage;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(hitTargets.Count);
    }

    public void AddWbuff(buff weaponBuff)
    {
        buffs.Add(weaponBuff);
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag(attackTarget))
            return;

        hitTargets.Add(hit.gameObject);

        if (!isAttack)
            return;

        EnemyAttributes a = hit.gameObject.GetComponent<EnemyAttributes>();

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
    void OnTriggerExit(Collider other)
    {
        if (hitTargets.Contains(other.gameObject))
            hitTargets.Remove(other.gameObject);
    }

    public void HurtHitTargets()
    {
        foreach(GameObject t in hitTargets)
        {
            EnemyAttributes a = t.GetComponent<EnemyAttributes>();

            //if (a.isBlocking)
            //{
            //    anim.SetTrigger("blocked");
            //    return;
            //}
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
        if(d == 0)
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
            if(d == 0)
                currentDamage.type = damageType.chi;
            else
                currentDamage.type = damageType.physical;
        }
        if (d == 0)
            currentDamage.pDamage = weaponDamage * (1 + attr.attrGet(4) / 50);
        else
            currentDamage.cDamage = 0;
    }

    public void ResetWholeDamage()
    {
        currentDamage = baseDamage;
    }

    public List<GameObject> GetHitTargets()
    {
        return hitTargets;
    }

    public string GetTargetLabel()
    {
        return attackTarget;
    }
}
