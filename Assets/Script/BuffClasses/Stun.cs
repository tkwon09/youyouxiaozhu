using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stun : Buff, setbuffparam {

    Transform target;

    public Stun() : base(buffType.Impair, true)
    {

    }

    protected override void UpdateFunction()
    {
        return;
    }

    protected override void StartFunction()
    {
        target = transform.root;
        if (target.CompareTag("Player"))
            target.GetComponent<CombatControl>().SetDisable(0);
        else
            target.GetComponent<EnemyBehavior>().SetDisable(0);
    }

    protected override void EndFunction()
    {
        if (target.CompareTag("Player"))
            target.GetComponent<CombatControl>().ResetDisable(0);
        else
            target.GetComponent<EnemyBehavior>().ResetDisable(0);
    }

    void setbuffparam.setTime(float durtime)
    {
        time = durtime;
    }

    // Use this for initialization
    //public override void Start()
    //{

    //}

    // Update is called once per frame
 //   protected override void Update ()
 //   {
 //       base.Update();
 //       UpdateFunction();
	//}

}
