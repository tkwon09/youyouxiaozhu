using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chi : MonoBehaviour
{
    bool isFriendly;
    Element element;
    int damage;
    string attackTarget;
    List<buff> buffs = new List<buff>();

	// Use this for initialization
	void Start ()
    {
        if (isFriendly)
            attackTarget = "Enemy";
        else
            attackTarget = "Player";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetChi(Element ele, int d, bool isfriendly = true)
    {
        element = ele;
        damage = d;
        isFriendly = isfriendly;
    }

    public void AddChiBuff(buff b)
    {
        buffs.Add(b);
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag(attackTarget))
            return;
        EnemyAttributes a = hit.gameObject.GetComponent<EnemyAttributes>();
        if (a)
        {
            damage currentDamage = new damage(damageType.chi,0,damage, element);
            a.TakeDamage(currentDamage);
        }
        foreach(buff b in buffs)
        {
            a.AddBuff(b);
        }
    }

}
