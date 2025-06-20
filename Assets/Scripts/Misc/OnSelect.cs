using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnSelect : MonoBehaviour
{
    TMP_InputField inputField;

    private void Start()
    {
        inputField = this.gameObject.GetComponent<TMP_InputField>();

    }

    public void SelectEvent()
    {
        this.gameObject.GetComponent<EventMiddleMan>().SetOriginalInputFieldText(inputField.text);
        this.gameObject.GetComponent<KeyboardVisibility>().enabled = true;
    }
}
