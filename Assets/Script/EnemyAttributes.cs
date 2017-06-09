using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyAttributes : MonoBehaviour {

    public int maxHealth;
    public int maxChi;
    public int IP;

    int health;
    int chi;

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
    public int[] elementResistance = new int[5];

    static Color32 highlightColor = new Color32(255, 255, 255, 255);
    static Color32 defaultColor = new Color32(150, 150, 150, 130);

    // Use this for initialization
    void Start()
    {
        buffs = transform.Find("Buffs");
        healthBar = transform.Find("EnemyGUI").GetChild(0).GetComponent<Image>();
        chiBar = transform.Find("EnemyGUI").GetChild(1).GetComponent<Image>();
        behavior = GetComponent<EnemyBehavior>().behavior;
        health = maxHealth;
        chi = maxChi;
        UpdateUI();
        ResetUI();
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
                return IP;
            case 3:
                return health;
            case 4:
                return chi;
        }
        return -1;
    }

    public void AddBuff(buff buff)
    {
        GameObject temp = new GameObject(buff.buffName);
        temp.transform.parent = buffs;
        Type buffclass = Type.GetType(buff.buffName);
        temp.AddComponent(buffclass);
        setbuffparam inter = (setbuffparam)temp.GetComponent(buffclass);
        if (buff.isTemp)
            inter.setTime(buff.time);
        GameObject tempv = Resources.Load<GameObject>("Visual/" + buff.buffName);
        if(tempv)
            Instantiate(tempv, transform.position + (GetComponent<CharacterController>().height * 2.3f) * Vector3.up, Quaternion.identity, temp.transform);
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

    public void TakeDamage(damage d)
    {
        int totalPD = 0;
        int chiDamage = 0;
        if (d.type == damageType.chi || d.type == damageType.blended)
        {
            chiDamage = Mathf.Clamp((int)(d.cDamage * (1 - elementResistance[(int)d.element] / 100f)) - (int)(0.3f * IP), 0, 2 * d.cDamage);
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

    public bool Decrease(int index, int amount = 1, bool self = false)
    {
        switch (index)
        {
            case 0:
                if (health <= amount)
                {
                    if (self)
                        return false;
                    else
                    {
                        health = 0;
                        behavior.Die();
                        UpdateUI(0);
                        return false;
                    }
                }
                else
                {
                    health -= amount;
                    UpdateUI(0);
                    return true;
                }
            case 1:
                if (chi <= amount)
                {
                    if (self)
                    {
                        return false;
                    }
                    else
                    {
                        chi = 0;
                        UpdateUI(1);
                        return false;
                    }
                }
                else
                {
                    chi -= amount;
                    UpdateUI(1);
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

    public void HighLightUI()
    {
        healthBar.color = highlightColor;
        chiBar.color = highlightColor;
    }

    public void ResetUI()
    {
        healthBar.color = defaultColor;
        chiBar.color = defaultColor;
    }

    public EnemyAttack GetAttack()
    {
        return attack;
    }

    void OnTriggerEnter(Collider hit)
    {

    }

}
