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

    InnerKF.plus IKFplus;

    // Use this for initialization
    void Start()
    {
        buffs = transform.Find("Buffs");
        //IKFplus = transform.parent.Find("IKF").gameObject.GetComponent<InnerKF>().levelPlus;
        maxHealth = InnerKF.initialHealth + IKFplus.healthPlus;
        maxStamina = InnerKF.initialStamina + IKFplus.staminaPlus;
        maxChi = IKFplus.chiPlus;
        IP = IKFplus.IPPlus;
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

    public int[] attrSave()
    {
        int[] save = new int[] {KP,fame,dressing};
        return save;
    }

    public void attrLoad(int[] data)
    {
        KP = data[0];
        fame = data[1];
        dressing = data[2];
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
