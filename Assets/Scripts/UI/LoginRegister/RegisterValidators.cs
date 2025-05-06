using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterValidators : MonoBehaviour
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
            string specialCharacterPattern = @"[^a-zA-Z0-9\s]";

            if (!input.Equals(""))
            {
                if (Regex.IsMatch(input, specialCharacterPattern))
                {
                    errorTMP.text = "The username contains special characters!";
                    Register.usernameCheck(true);
                }
                else
                {
                    Register.usernameCheck(false);
                    errorTMP.text = "";
                }
            }
            else
            {
                Register.usernameCheck(true);          
                errorTMP.text = "";
            }

        }
        else if (label.Equals("Password:"))
        {
            string missingCharPattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s])(?=.*[A-Z]).+$";

            if (!input.Equals(""))
            {
                if (!Regex.IsMatch(input, missingCharPattern))
                {
                    errorTMP.text = "It must have an uppercase letter, a number and a special character!";
                    Register.passwordCheck(true);
                }
                else if (input.Length <= 5)
                {
                    errorTMP.text = "Password must have 6 or more characters!";
                    Register.passwordCheck(true);
                }
                else
                {
                    Register.passwordCheck(false);
                    errorTMP.text = "";
                }
            }
            else
            {
                Register.passwordCheck(true);
                if (!errorTMP.text.Equals("Field is empty!"))
                {
                    errorTMP.text = "";
                }
            }
        }
        else if (label.Equals("Email:"))
        {
            string emailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)+$";

            if (!input.Equals(""))
            {
                if (!Regex.IsMatch(input, emailPattern))
                {
                    errorTMP.text = "Email is not valid!";
                    Register.emailCheck(true);
                }
                else
                {
                    Register.emailCheck(false);
                    errorTMP.text = "";
                }
            }
            else
            {
                Register.emailCheck(true);
                errorTMP.text = "";            
            }
        }
        else
        {
            GameObject password = GameObject.FindGameObjectWithTag("confirmpass");
            TMP_InputField passwordInput = password.GetComponent<TMP_InputField>();

            if (!input.Equals(""))
            {
                if (!input.Equals(passwordInput.text))
                {
                    errorTMP.text = "Passwords don't match!";
                    Register.confirmPassCheck(true);
                }
                else
                {
                    Register.confirmPassCheck(false);
                    errorTMP.text = "";
                }
            }
            else
            {
                Register.confirmPassCheck(true);
                if (!errorTMP.text.Equals("Field is empty!"))
                {
                    errorTMP.text = "";
                }
            }

        }
    }
}
