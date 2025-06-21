using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnValueChanged : MonoBehaviour
{
    private TMP_InputField inputField;
    private void Start()
    {
        inputField = this.gameObject.GetComponent<TMP_InputField>();
    }

    public void OnValueChangedEvent()
    {        
        //Debug.Log("CurrentChangedText: " + inputField.text);
        this.gameObject.GetComponent<EventMiddleMan>().SetCurrentInputFieldText(inputField.text);
    }
}
