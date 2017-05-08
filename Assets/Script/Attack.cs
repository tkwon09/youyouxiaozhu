using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : MonoBehaviour
{

    bool isFriendly;
    public bool isAttack;
    public bool isTargetHit;
    public bool isAnimating;
    public bool getBlocked;

    string attackTarget;
    public List<GameObject> hitTargets = new List<GameObject>();
    public int maxAttackPhase;

    damageType currentDamageType;
    Animator animator;
    State state;
    Attributes attr;
    GameObject player;
    public int baseDamage;

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
        animator = transform.parent.parent.parent.gameObject.GetComponent<Animator>();
        state = transform.parent.parent.parent.gameObject.GetComponent<State>();
        attr = transform.parent.parent.parent.parent.parent.GetComponent<Attributes>();
        player = transform.parent.parent.parent.parent.parent.gameObject;
        maxAttackPhase = attr.attrGet(4)/1;
        currentDamage = new damage(damageType.physical, baseDamage*(1+attr.attrGet(4)/50));
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void AddWbuff(buff weaponBuff)
    {
        buffs.Add(weaponBuff);
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!isAttack || getBlocked || !hit.gameObject.CompareTag(attackTarget))
            return;
        Attributes a = hit.gameObject.GetComponent<Attributes>();
        if (a)
        {
            isTargetHit = true;
            hitTargets.Add(hit.gameObject);
            a.TakeDamage(currentDamage);
            if (wbuffs.Count != 0)
            {
                foreach (wbuff item in wbuffs)
                {
                    if(Random.value <= item.probability)
                        buffs.Add(item.buffToAdd);
                }
            }

            if (buffs.Count != 0)
            {
                for(int i = buffs.Count-1;i>=0;i--)
                {
                    a.AddBuff(buffs[i]);
                    buffs.RemoveAt(i);
                }
            }
        }
        isTargetHit = false;
    }

    public void SetCurrentDamage(damageType dt)
    {
        currentDamageType = dt;
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
