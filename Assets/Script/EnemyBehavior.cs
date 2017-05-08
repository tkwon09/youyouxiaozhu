using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public Animator anim;
    public enum EnemyType { idle, attack, guard};
    public EnemyType type;
    public bool functioning;


	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Behave());
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator Behave()
    {
        while(functioning)
        {
            yield return new WaitForSeconds(3);
            switch(type)
            {
                case EnemyType.attack:
                    anim.SetTrigger("attack1");
                    break;
                case EnemyType.guard:
                    anim.SetTrigger("guard");
                    break;
                default:
                    break;
            }
        }
    }
}
