using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stun : Buff, setbuffparam {

    public Stun() : base(buffType.Impair, true)
    {

    }

    public override void UpdateFunction()
    {
        //transform.parent.parent.gameObject.GetComponent<MovementScript>().canMove = false;
        return;
    }

    public override void StartFunction()
    {
        
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
    public override void Update ()
    {
        base.Update();
        UpdateFunction();
	}

    private void OnDestroy()
    {
        //transform.parent.parent.gameObject.GetComponent<MovementScript>().canMove = true;
    }
}
