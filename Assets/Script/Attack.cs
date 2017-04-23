using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : MonoBehaviour
{
    enum AttackType {Sword,Chi};
    AttackType attackType;
    int damage;
    bool isFriendly;
    public bool isAttack;
    public bool isTargetHit;

    string attackTarget;
    public List<GameObject> hitTargets = new List<GameObject>();

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

    public List<wbuff> wbuffs = new List<wbuff>();
    public List<buff> buffs = new List<buff>();

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
        if (!isAttack)
            return;
        if (hit.gameObject.CompareTag(attackTarget))
        {
            Attributes a = hit.gameObject.GetComponent<Attributes>();
            if (a)
            {
                Debug.Log(1);
                isTargetHit = true;
                hitTargets.Add(hit.gameObject);

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
        }
        isTargetHit = false;
    }

    public List<GameObject> GetHitTargets()
    {
        return hitTargets;
    }
}
