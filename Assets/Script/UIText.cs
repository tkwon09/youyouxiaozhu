using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIText : MonoBehaviour {

    public Text targetText;

    public string className;
    public string variable;
    public string prefix;
    public string suffix;

    Type t;

	// Use this for initialization
	void Start ()
    {
        t = Type.GetType(className);
        var refClass = GetComponent(t);
        string content = refClass.GetType().GetField(variable).GetValue(refClass).ToString();
        targetText.text = content;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void UpdateUIText()
    {
        var refClass = GetComponent(t);
        string content = refClass.GetType().GetField(variable).GetValue(refClass).ToString();
        targetText.text = content;
    }
}
