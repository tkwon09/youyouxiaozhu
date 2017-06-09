using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChiBarDisplay : MonoBehaviour {

    float spinSpeed = 0f;
    float spinAcc;
    const float spinResis = 1f;
    const float steadSpinSpeed = 4f;
    float currentSpinResis = 5f;
    float maxSpinSpeed = 20f;
    float minSpinSpeed = 0f;

    bool elementOn;
    bool elementNull;
    float mouseBound = 0.5f;

    public GameObject element;
    public ParticleSystem chiElementPromt;
    public CameraScript cams;
    public Attributes attr;
    int currentOnElement;

    public static Color32[] elementColors = new Color32[5] { new Color32(255, 255, 75, 255), new Color32(30, 255, 20, 255), new Color32(75, 155, 255, 255), new Color32(255, 10, 10, 255), new Color32(115, 70, 0, 255) };

    // Use this for initialization
    void Start ()
    {
        elementNull = true;
        currentOnElement = 2;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        spinSpeed = Mathf.Clamp(spinSpeed - spinResis * Time.fixedDeltaTime,minSpinSpeed,maxSpinSpeed);
		if(spinSpeed > 0)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - spinSpeed);

        if(elementOn)
        {
            if (-0.4f < Input.GetAxis("Mouse X") && Input.GetAxis("Mouse X") < 0.4f)
                mouseBound = 0.5f;
            if (Input.GetAxis("Mouse X") > mouseBound || Input.GetAxis("Mouse ScrollWheel") * 10 < -0.5f)
            {
                mouseBound = 1.5f;
                ResetElement();
                HighlightElement(currentOnElement + 1);
            }
            if (Input.GetAxis("Mouse X") < -mouseBound || Input.GetAxis("Mouse ScrollWheel") * 10 > 0.5f)
            {
                mouseBound = 1.5f;
                ResetElement();
                HighlightElement(currentOnElement - 1);
            }
        }
	}

    public void AddAcc(float acc)
    {
        spinAcc += acc;
    }

    public void AddSpeed(float speed)
    {
        spinSpeed += speed;
    }

    public void SteadySpin()
    {
        spinSpeed = steadSpinSpeed;
        minSpinSpeed = steadSpinSpeed;
        chiElementPromt.gameObject.SetActive(true);
    }

    public void NormalSpin()
    {
        minSpinSpeed = 0;
        chiElementPromt.gameObject.SetActive(false);
    }

    public void ShowElement()
    {
        element.SetActive(true);
        elementOn = true;
        cams.UIOn = true;
        if(!elementNull)
            HighlightElement(currentOnElement);
    }

    public void HideElement()
    {
        ResetElement();
        element.SetActive(false);
        cams.UIOn = false;
        elementOn = false;
    }

    void HighlightElement(int index)
    {
        index = Mathf.Clamp(index, 0, 4);
        currentOnElement = index;
        attr.ChangeCurrentElement(currentOnElement);
        element.transform.GetChild(currentOnElement).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        chiElementPromt.startColor = elementColors[currentOnElement];
        elementNull = false;
    }

    void ResetElement()
    {
        element.transform.GetChild(currentOnElement).GetComponent<Image>().color = new Color32(150, 150, 150, 130);
    }

    public int GetCurrentElement()
    {
        if (elementNull)
            return -1;
        else
            return currentOnElement;
    }

}
