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

    public override void UpdateFunction()
    {
        
    }

    public override void StartFunction()
    {
        buff QStun = new buff("Stun", true, 2.5f);
        wbuff QWBuff = new wbuff(QStun,stunRate);
        attack.wbuffs.Add(QWBuff);
    }

    void setbuffparam.setTime(float durtime)
    {
        time = durtime;
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Player = GameObject.Find("Player");
        attack = Player.GetComponent<Attributes>().GetAttack();
        StartFunction();
    }

    // Update is called once per frame
    public override void Update ()
    {
		
	}
}
