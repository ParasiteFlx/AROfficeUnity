using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class PlaneSelection : MonoBehaviour { 
   
    private TextMeshPro buttonText;
    private Touch touch;
   
    // Start is called before the first frame update
    void Start()
    {  
        GameObject buttonTextGameObject = GameObject.FindGameObjectWithTag("debug");
        buttonText = buttonTextGameObject.GetComponent<TextMeshPro>();
   
    }

    // Update is called once per frame
    void Update()
    {
        planeSelection();
    }

    private void planeSelection()
    {
       // buttonText.text = "planeSelection Entered";

        if(Input.touchCount == 1)
        {  
            touch = Input.GetTouch(0);

            buttonText.text = "Tap Count este " + touch.tapCount.ToString();

            if(touch.tapCount == 2)
            {
                touch.tapCount = 0;
                LineRenderer lineRenderer = this.gameObject.GetComponent<LineRenderer>();
                Color colour = new Color(120 / 255f, 23 / 255f, 32 / 255f);
            
                buttonText.text = this.gameObject.GetType().Name.ToString();
                if (ColorUtility.ToHtmlStringRGBA(lineRenderer.startColor) == ColorUtility.ToHtmlStringRGBA(colour))
                {
                    this.gameObject.SetActive(false);
                    this.gameObject.GetComponent<LineRenderer>().startColor = Color.black;
                    this.gameObject.GetComponent<LineRenderer>().endColor = Color.black;
                }
                else
                {
                    this.gameObject.GetComponent<LineRenderer>().startColor = colour ;
                    this.gameObject.GetComponent<LineRenderer>().endColor =colour;
                }
                


            }
          

        }
    }
}

