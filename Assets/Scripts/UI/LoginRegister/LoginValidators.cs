using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginValidators : MonoBehaviour
{
    string label, input;
    [SerializeField]
    TextMeshProUGUI errorTMP;
    TMP_InputField inputField;

    private void Start()
    {
        Transform parent = gameObject.transform.parent;
        label = parent.gameObject.GetComponent<TextMeshProUGUI>().text;
        inputField = gameObject.gameObject.GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        input = inputField.text;
        Validator();
    }

    private void Validator()
    {
        if (label.Equals("Username:"))
        {
          
            if (input.Equals(""))
            {               
               errorTMP.text = "The username field is empty!";                              
            }
            else
            {
                errorTMP.text = "";
            }

        }
        else if (label.Equals("Password:"))
        {
            
            if (input.Equals(""))
            {
                errorTMP.text = "The password field is empty!";
            }
            else
            {
                errorTMP.text = "";
            }

        }
    }
}
