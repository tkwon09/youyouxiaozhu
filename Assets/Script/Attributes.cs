using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public Transform rotationtransform;
    public Image healthBar;
    public Image chiBar;
    public Image staminaBar;
    public GameObject swordChi;
    public GameObject fcChi;
    public GameObject healthPop;
    public GameObject chiPop;
    public bool chiOn;

    InnerKF.plus IKFplus;

    public enum Element { gold, wood, water, fire, earth};
    public int[] elementMaster = new int[5];
    public Element currentElement;
    float currentElementMastery;

    // Use this for initialization
    void Start()
    {
        buffs = transform.Find("Buffs");
        currentElementMastery = elementMaster[(int)currentElementMastery];
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
            chiDamage = d.cDamage - (int)(0.3f * IP);
            totalPD = d.pDamage +  chiDamage;
        }
        else
            totalPD = d.pDamage;
        if (totalPD > 0)
            animator.SetTrigger("gethurt");
        Decrease(0, totalPD);
        GameObject hpop = Instantiate(healthPop, healthBar.transform) as GameObject;
        hpop.GetComponent<Text>().text = "-" + totalPD.ToString();
        Destroy(hpop, 1.25f);
        if (chiDamage != 0)
        {
            Decrease(1, chiDamage);
            GameObject cpop = Instantiate(chiPop,chiBar.transform) as GameObject;
            cpop.GetComponent<Text>().text = "-" + chiDamage.ToString();
            Destroy(cpop, 1.25f);
        }
    }

    public void DamagePop(int index)
    {
        
    }

    public bool StartChi()
    {
        if (chi >= 5)
        {
            chiOn = true;
            attack.AddCurrentDamage(1,(int)(IP * 0.5f));
            StartCoroutine(ChiDec());
            return true;
        }
        else
            return false;
    }

    public void TurnChiOn()
    {
        swordChi.SetActive(true);
    }

    public void EndChi()
    {
        chiOn = false;
        swordChi.SetActive(false);
        attack.ResetWholeDamage();
    }

    IEnumerator ChiDec()
    {
        while(chiOn)
        {
            yield return new WaitForSeconds(1);
            if (!Decrease(1,10))
            {
                chiOn = false;
                attack.ResetWholeDamage();
            }
        }
    }

    public bool UseChiSpell(int index)
    {
        switch(index)
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

    public void CreateFrontCastChi()
    {
        GameObject tempChi = Instantiate(fcChi, transform.position + 2.5f * (rotationtransform.forward) + 1.5f * Vector3.up, rotationtransform.rotation);
        tempChi.AddComponent<Chi>();
        tempChi.GetComponent<Chi>().SetChi(currentElement,(int)(currentElementMastery / 100.0f * IP));
        Destroy(tempChi, 1);
    }

    bool Decrease(int index, int amount = 1)
    {
        switch (index)
        {
            case 0:
                if (health <= amount)
                {
                    health = 0;
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

    void UpdateUI(int index = 0)
    {
        switch (index)
        {
            case 0:
                healthBar.fillAmount = (float)health / maxHealth;
                break;
            case 1:
                chiBar.fillAmount = (float)chi / maxChi;
                break;
            case 2:
                staminaBar.fillAmount = (float)stamina / maxStamina;
                break;
            default:
                healthBar.fillAmount = (float)health / maxHealth;
                chiBar.fillAmount = (float)chi / maxChi;
                staminaBar.fillAmount = (float)stamina / maxStamina;
                break;
        }
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
