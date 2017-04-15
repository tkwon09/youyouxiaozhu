using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerKF : MonoBehaviour
{
    public string IKFname;
    int level;

    // unchanged data
    public Image icon;
    public string desc;
    public struct plus
    {
        int healthPlus;
        int chiPlus;
        int IPPlus;
        int staminaPlus;
    }
    public plus[] levelPlus = new plus[9];


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
