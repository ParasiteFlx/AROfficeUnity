using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionButtonText : MonoBehaviour
{
    private TextMeshPro buttonText;
    private int transitionType;
    private string[] buttonTexts = { "None", "Simple", "Complex" };
    // Start is called before the first frame update
    void Start()
    {
        buttonText = transform.GetComponent<TextMeshPro>();
        transitionType = Options.Instance().GetTransitionType();
        buttonText.text = buttonTexts[transitionType];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
