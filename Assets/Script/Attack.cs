using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    enum AttackType {Sword,Chi};
    AttackType attackType;
    int damage;
    public struct wbuff
    {
        public string buffName;
        public bool isTemp;
        public float time;
    }
    public List<wbuff> wbuffs;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
