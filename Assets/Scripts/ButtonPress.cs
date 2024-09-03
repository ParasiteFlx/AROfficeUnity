using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro phaseDisplayText;
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = 0.5f;

    private void Start()
    {
       
    }

    void Update()
    {
        theTouch = Input.GetTouch(0);
        if (Input.touchCount > 0)
        {
            if (theTouch.phase == TouchPhase.Ended)
            {
                phaseDisplayText.text = theTouch.phase.ToString();
                timeTouchEnded = Time.time;
            }
            else if (Time.time - timeTouchEnded > displayTime)
            {
                phaseDisplayText.text = theTouch.phase.ToString();
                timeTouchEnded = Time.time;
            }
        }
        else if(Time.time - timeTouchEnded > displayTime)
        {
            phaseDisplayText.text = "";
        }
    }
}
