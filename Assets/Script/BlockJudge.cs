using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockJudge : MonoBehaviour
{

    Attack attack;

    // Use this for initialization
    void Start()
    {
        attack = transform.parent.GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider hit)
    {
        if (!attack.isAttack && !attack.isAnimating || attack.isTargetHit || !hit.gameObject.CompareTag(attack.GetTargetLabel()))
            return;

        MovementScript m = hit.gameObject.GetComponent<MovementScript>();

        if (m && m.blocking)
        {
            attack.getBlocked = true;
            
        }
        attack.getBlocked = false;
    }
        
}
