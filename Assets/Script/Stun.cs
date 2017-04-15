using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stun : Buff, setbuffparam {

    //public Stun(float ti) : base(buffType.Impair,true,ti)
    //{

    //}

    public override void Function()
    {
        transform.parent.parent.gameObject.GetComponent<MovementScript>().canMove = false;
        return;
    }

    // Use this for initialization
    //public override void Start()
    //{

    //}

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();
        Debug.Log(counter);
        Function();
	}

    void setbuffparam.setisTemp(bool istemp)
    {
        isTemp = istemp;
    }
    void setbuffparam.setTime(float durtime)
    {
        time = durtime;
    }
    private void OnDestroy()
    {
        transform.parent.parent.gameObject.GetComponent<MovementScript>().canMove = true;
    }
}
