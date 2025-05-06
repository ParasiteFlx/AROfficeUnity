using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidatorsEnabler : MonoBehaviour
{
    
    RegisterValidators registerValidators;

    private void Start()
    {
       
        registerValidators = gameObject.GetComponent<RegisterValidators>();
    }

    public void EnableScript()
    {        
            if(registerValidators != null)
            {
                registerValidators.enabled = true;
            }      
    }

    public void DisableScript()
    {
        if (registerValidators != null)
            {
                registerValidators.enabled = false;
            }
        
    }

}
