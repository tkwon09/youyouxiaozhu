﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyAttributes : MonoBehaviour {

    public int maxHealth;
    public int maxChi;
    public int maxStamina;
    public int IP;
    public int KP;

    int health;
    int chi;
    int stamina;

    Transform buffs;
    Image healthBar;
    Image chiBar;
    EnemyBehaviors behavior;
    public string enemyType;
    public string enemyName;
    public EnemyAttack attack;
    public DataManager dataManager;
    public GameObject healthPop;
    public GameObject chiPop;

    // Use this for initialization
    void Start()
    {
        buffs = transform.Find("Buffs");
        healthBar = transform.Find("EnemyGUI").GetChild(0).GetComponent<Image>();
        chiBar = transform.Find("EnemyGUI").GetChild(1).GetComponent<Image>();
        behavior = GetComponent<EnemyBehavior>().behavior;
        health = maxHealth;
        chi = maxChi;
        stamina = maxStamina;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int attrGet(int index)
    {
        if (index < 0 || index > 7)
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
                return health;
            case 6:
                return chi;
            case 7:
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
        if (buff.isTemp)
            inter.setTime(buff.time);
        Instantiate(Resources.Load<GameObject>("Visual/" + buff.buffName), transform.position + Vector3.up * 3.5f, Quaternion.identity, temp.transform);
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
            chiDamage = d.cDamage - (int)(0.3f * IP);
            totalPD = d.pDamage + chiDamage;
        }
        else
            totalPD = d.pDamage;
        if (totalPD > 0)
            behavior.GetHurt();
        if (totalPD > 10)
            totalPD = (int)(totalPD * (1 + Random.value * 0.08f + -0.04f));
        Decrease(0, totalPD);
        GameObject hpop = Instantiate(healthPop, healthBar.transform) as GameObject;
        hpop.GetComponent<Text>().text = "-" + totalPD.ToString();
        Destroy(hpop, 1.25f);
        if (chiDamage != 0)
        {
            Decrease(1, chiDamage);
            GameObject cpop = Instantiate(chiPop, chiBar.transform) as GameObject;
            cpop.GetComponent<Text>().text = "-" + chiDamage.ToString();
            Destroy(cpop, 1.25f);
        }
    }

    public void DamagePop(int index)
    {

    }

    public bool UseChiSpell(int index)
    {
        switch (index)
        {
            case 0:
                if (chi < 20)
                    return false;
                else
                {
                    Decrease(1, 20);
                    return true;
                }
            default:
                return false;
        }
    }

    bool Decrease(int index, int amount = 1)
    {
        switch (index)
        {
            case 0:
                if (health <= amount)
                {
                    health = 0;
                    behavior.Die();
                    UpdateUI();
                    return false;
                }
                else
                {
                    health -= amount;
                    UpdateUI();
                    return true;
                }
            case 1:
                if (chi <= amount)
                {
                    chi = 0;
                    UpdateUI(1);
                    return false;
                }
                else
                {
                    chi -= amount;
                    UpdateUI(1);
                    return true;
                }
            case 2:
                if (stamina <= amount)
                {
                    stamina = 0;
                    UpdateUI(2);
                    return false;
                }
                else
                {
                    stamina -= amount;
                    UpdateUI(2);
                    return true;
                }
            default:
                return true;
        }
    }

    void UpdateUI(int index = 2)
    {
        switch (index)
        {
            case 0:
                healthBar.fillAmount = (float)health / maxHealth;
                break;
            case 1:
                chiBar.fillAmount = (float)chi / maxChi;
                break;
            default:
                healthBar.fillAmount = (float)health / maxHealth;
                chiBar.fillAmount = (float)chi / maxChi;
                break;
        }
    }

    void OnTriggerEnter(Collider hit)
    {

    }

}
