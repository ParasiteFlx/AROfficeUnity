using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAreaEvents : MonoBehaviour
{
    TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField = gameObject.GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if(inputField.isFocused)
        {
            inputField.placeholder.enabled = false;
        }
        else
        {
            if(inputField.text.Length == 0)
            inputField.placeholder.enabled = true;
        }
    }

}
