using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public buffType type;
    public bool isTemp;
    public float time;
    public float counter = -1f;

    // called in Update()
    protected abstract void UpdateFunction();
    // called in Start()
    protected abstract void StartFunction();
    protected abstract void EndFunction();
    public virtual void Disappear()
    {
        Destroy(gameObject, time);
    }

    public Buff(buffType ty, bool iT, float ti = 0)
    {
        type = ty;
        isTemp = iT;
        time = ti;
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        StartFunction();
        if (isTemp)
        {
            counter = time;
            Disappear();
        }
	}

    // Update is called once per frame
    protected virtual void Update ()
    {
        counter -= Time.deltaTime;
	}

    protected virtual void OnDestroy()
    {
        EndFunction();
    }
}
