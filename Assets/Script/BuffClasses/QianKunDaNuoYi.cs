using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QianKunDaNuoYi : Buff, setbuffparam
{
    Attack attack;
    bool Attacking;
    bool TargetHit;
    GameObject Player;
    float stunRate = 0.33f;

    public QianKunDaNuoYi() : base(buffType.Enhence, false)
    {

    }

    protected override void UpdateFunction()
    {

    }

    protected override void StartFunction()
    {
        Player = GameObject.Find("Player");
        attack = Player.GetComponent<Attributes>().GetAttack();
        buff QStun = new buff("Stun", true, 8f);
        wbuff QWBuff = new wbuff(QStun,stunRate);
        attack.wbuffs.Add(QWBuff);
    }

    protected override void EndFunction()
    {
        
    }

    void setbuffparam.setTime(float durtime)
    {
        time = durtime;
    }

    // Use this for initialization
    //protected override void Start()
    //{

    //}

    // Update is called once per frame
 //   protected override void Update ()
 //   {
		
	//}
}
