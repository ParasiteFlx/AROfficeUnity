using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidatorsEnabler : MonoBehaviour
{
    LoginValidators loginValidators;
    RegisterValidators registerValidators;

    private void Start()
    {
        loginValidators = gameObject.GetComponent<LoginValidators>();
        registerValidators = gameObject.GetComponent<RegisterValidators>();
    }

    public void EnableScript()
    {
        if (loginValidators != null)
        {
            loginValidators.enabled = true;
        }
        else
        {
            if(registerValidators != null)
            {
                registerValidators.enabled = true;
            }
        }
    }

    public void DisableScript()
    {
        if (loginValidators != null)
        {
            loginValidators.enabled = false;
        }
        else
        {
            if (registerValidators != null)
            {
                registerValidators.enabled = false;
            }
        }
    }

}
