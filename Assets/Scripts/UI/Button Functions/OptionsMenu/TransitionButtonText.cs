using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionButtonText : MonoBehaviour
{
    private TextMeshPro buttonText;
    [SerializeField]
    private TextMeshPro buttonNameTMP;
    private bool firstTimeStart = true;

    void Start()
    {
        buttonText = transform.GetComponent<TextMeshPro>();
         Debug.Log("Am setat initial textul ");
        buttonText.text = SetText();
        firstTimeStart = false;
    }

    void OnEnable()
    {
        if (!firstTimeStart)
        {
            Debug.Log("Am setat la OnEnable textul ");
            Transitions.Instance().applyState = true;
            buttonText.text = SetText();
        }
    }
    
    private string SetText()
    {
        string text = "";
        string buttonName = buttonNameTMP.text;
        if (buttonName.Equals("Transition Type"))
        {
            text = Options.Instance().GetTransitionTypeName();
        }
        else if (buttonName.Equals(""))
        {
            text = "Apply";
        }
         return text;
       
    }
}
