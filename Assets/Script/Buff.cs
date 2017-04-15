﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface setbuffparam
{
    void setisTemp(bool istemp);
    void setTime(float durtime);
}

public abstract class Buff : MonoBehaviour
{
    public enum buffType {Enhence, Impair};
    public buffType type;
    public bool isTemp;
    public float time;
    public float counter = -1f;

    // called in Update()
    public abstract void Function();
    // called in Start()
    public virtual void Disappear()
    {
            Destroy(gameObject, time);
    }

    //public Buff(buffType ty, bool iT, float ti)
    //{
    //    type = ty;
    //    isTemp = iT;
    //    time = ti;
    //}

	// Use this for initialization
	public virtual void Start ()
    {
        if (isTemp)
        {
            counter = time;
            Disappear();
        }
	}

    // Update is called once per frame
    public virtual void Update ()
    {
        counter -= Time.deltaTime;
	}
}
