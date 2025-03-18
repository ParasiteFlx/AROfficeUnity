using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionButtonText : MonoBehaviour
{
    private TextMeshPro buttonText;
    private bool firstTimeStart = true;

    void Start()
    {
        buttonText = transform.GetComponent<TextMeshPro>();
        buttonText.text = Options.Instance().GetTransitionTypeName();
        firstTimeStart = false;
    }

    void OnEnable()
    {
        if (!firstTimeStart)
        {
            buttonText.text = Options.Instance().GetTransitionTypeName();
        }
    }
       
}
