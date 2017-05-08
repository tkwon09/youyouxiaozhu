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
        if (!CompareTag("Player"))
            UpdateIKF(ikfName, level);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void UpdateIKF(string ikfName, int level)
    {
        StartCoroutine(WaitForDict());
        
    }
    IEnumerator WaitForDict()
    {
        yield return new WaitForSeconds(0.5f);
        DataManager dm = GameObject.Find("DataManager").GetComponent<DataManager>();
        dm.AddIKF(ikfName, level, this, gameObject.GetComponent<Attributes>());
    }
}
