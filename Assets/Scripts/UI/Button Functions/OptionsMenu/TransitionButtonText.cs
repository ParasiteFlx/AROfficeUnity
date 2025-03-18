using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionButtonText : MonoBehaviour
{
    private TextMeshPro buttonText;

    void Awake()
    {
        buttonText = transform.GetComponent<TextMeshPro>();
    }

    void OnEnable()
    {   
        buttonText.text = Options.Instance().GetTransitionTypeName();
    }
}
