using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Attributes : MonoBehaviour
{
    public const int initialHealth = 10;
    public const int initialChi = 10;
    public const int initialStamina = 10;

    int maxHealth;
    int maxChi;
    int maxStamina;
    int IP;
    public int KP;
    public int fame;
    public int dressing;

    int health;
    int chi;
    int stamina;

    Transform buffs;
    public Attack attack;
    public DataManager dataManager;
    public Animator animator;
    public Image Healthbar;

    InnerKF.plus IKFplus;

    // Use this for initialization
    void Start()
    {
        buffs = transform.Find("Buffs");
        //StartCoroutine(DebugAttr());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DebugAttr()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("HP: " + maxHealth);
        Debug.Log("Chi: " + maxChi);
        Debug.Log("Stamina: " + maxStamina);
        Debug.Log("IP: " + IP);
    }
    public void attrLoadIKF()
    {
        IKFplus = GetComponent<InnerKF>().levelPlus;
        maxHealth = initialHealth + IKFplus.healthPlus;
        maxStamina = initialStamina + IKFplus.staminaPlus;
        maxChi = initialChi + IKFplus.chiPlus;
        IP = IKFplus.IPPlus;
        health = maxHealth;
        chi = maxChi;
        stamina = maxStamina;
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

    public void attrChange(int index, int number)
    {
        if (index < 0 || index > 9)
            return;
        switch(index)
        {
            case 0:
                maxHealth = Mathf.Clamp(maxHealth + number, initialHealth, 99999);
                break;
            case 1:
                maxChi = Mathf.Clamp(maxChi + number, initialChi, 99999);
                break;
            case 2:
                maxStamina = Mathf.Clamp(maxStamina + number, initialStamina, 99999);
                break;
            case 3:
                IP = Mathf.Clamp(IP + number, 0, 9999);
                break;
            case 4:
                KP = Mathf.Clamp(KP + number, 0, 9999);
                break;
            case 5:
                fame = Mathf.Clamp(fame + number, 0, 9999);
                break;
            case 6:
                dressing = Mathf.Clamp(dressing + number, 0, 9999);
                break;
            case 7:
                health = Mathf.Clamp(health + number, 1, maxHealth);
                break;
            case 8:
                chi = Mathf.Clamp(chi + number, 0, maxChi);
                break;
            case 9:
                stamina = Mathf.Clamp(stamina + number, 1, maxStamina);
                break;
        }
    }

    public int attrGet(int index)
    {
        if (index < 0 || index > 6)
            return -1;
        switch (index)
        {
            case 0:
                return maxHealth;
            case 1:
                return maxChi;
            case 2:
                return maxStamina;
            case 3:
                return IP;
            case 4:
                return KP;
            case 5:
                return fame;
            case 6:
                return dressing;
            case 7:
                return health;
            case 8:
                return chi;
            case 9:
                return stamina;
        }
        return -1;
    }

    public void AddBuff(Attack.buff buff)
    {
        GameObject temp = new GameObject(buff.buffName);
        temp.transform.parent = buffs;
        Type buffclass = Type.GetType(buff.buffName);
        temp.AddComponent(buffclass);
        setbuffparam inter = (setbuffparam)temp.GetComponent(buffclass);
        if(buff.isTemp)
            inter.setTime(buff.time);
        Instantiate(Resources.Load<GameObject>("Visual/" + buff.buffName),transform.position+Vector3.up*3.5f,Quaternion.identity,temp.transform);
    }

    public void AddBuff(string name, bool istemp, float time = 0)
    {
        GameObject temp = new GameObject(name);
        temp.transform.parent = buffs;
        Type buffclass = Type.GetType(name);
        temp.AddComponent(buffclass);
        setbuffparam inter = (setbuffparam)temp.GetComponent(buffclass);
        if (istemp)
            inter.setTime(time);
    }

    public void TakeDamage(Attack.damage d)
    {
        int totalPD = 0;
        int chiDamage = 0;
        if (d.type == Attack.damageType.chi || d.type == Attack.damageType.blended)
        {
            chiDamage = d.cDamage - (int)(0.4f * IP);
            totalPD = d.pDamage +  chiDamage;
        }
        else
            totalPD = d.pDamage;
        if (totalPD > 0)
            animator.SetTrigger("gethurt");
        health -= totalPD;
        Healthbar.fillAmount = (float)health / maxHealth;
    }

    void OnTriggerEnter(Collider hit)
    {

    }

    public Attack GetAttack()
    {
        return attack;
    }

    public void PlayBlockAnim(int blocktype)
    {
        switch(blocktype)
        {
            case 0:
                animator.SetTrigger("UpBlock");
                break;
            case 1:
                animator.SetTrigger("MiddleBlock");
                break;
            case 2:
                animator.SetTrigger("JumpEvade");
                break;
        }
    }
}
