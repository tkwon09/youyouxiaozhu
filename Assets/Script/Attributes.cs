using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attributes : MonoBehaviour
{

    int maxHealth;
    int maxChi;
    int maxStamina;
    int health;
    int chi;
    int stamina;

    int IP;
    int KP;
    int fame;
    int dressing;

    Transform buffs;

    // Use this for initialization
    void Start()
    {
        buffs = transform.Find("Buffs");
        AddBuff("Stun", 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("harm"))
        {
            Attack a = hit.gameObject.GetComponent<Attack>();
            if(a)
            {
                if(a.wbuffs.Count!=0)
                {
                    foreach(Attack.wbuff buff in a.wbuffs)
                    {
                        AddBuff(buff.buffName, buff.time);
                    }
                }
            }
        }
    }

    void AddBuff(string name, float time)
    {
        GameObject temp = new GameObject(name);
        temp.transform.parent = buffs;
        Type buffclass = Type.GetType(name);
        temp.AddComponent(buffclass);
        setbuffparam inter = (setbuffparam)temp.GetComponent(buffclass);
        inter.setisTemp(true);
        inter.setTime(5f);
    }
}
