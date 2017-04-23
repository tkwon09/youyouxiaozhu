using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerKF : MonoBehaviour
{
    public string ikfName;
    public int level;

    // unchanged data
    public Image icon;
    public string desc;
    public struct plus
    {
        public int healthPlus;
        public int chiPlus;
        public int IPPlus;
        public int staminaPlus;
    }

    public plus levelPlus;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

}
