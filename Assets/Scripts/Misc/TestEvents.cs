using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestEvents : MonoBehaviour
{
    TMP_InputField inputField;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void OnSelectEvent()
    {
        Debug.Log("InputField Selected");
        Debug.Log("InputField Text: " + inputField.text);
       
    }

    public void OnDeselectEvent()
    {
        Debug.Log("InputField DeSelected");
        Debug.Log("InputField Text: " + inputField.text);
    }

    public void OnEndEditEvent()
    {
        Debug.Log("Edit Ended");
        Debug.Log("InputField Text: " + inputField.text);
    }
}
