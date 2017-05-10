using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chi : MonoBehaviour
{
    bool isFriendly;
    Attributes.Element element;
    int damage;
    string attackTarget;

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

    public void SetChi(Attributes.Element ele, int d, bool isfriendly = true)
    {
        element = ele;
        damage = d;
        isFriendly = isfriendly;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag(attackTarget))
            return;
        Attributes a = hit.gameObject.GetComponent<Attributes>();
        if (a)
        {
            Attack.damage currentDamage = new Attack.damage(Attack.damageType.chi,0,damage);
            a.TakeDamage(currentDamage);
        }
    }

}
