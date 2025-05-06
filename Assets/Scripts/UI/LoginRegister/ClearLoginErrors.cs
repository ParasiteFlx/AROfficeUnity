using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearLoginErrors : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI usernameError, passwordError;

    public void ClearUsernameError()
    {
        usernameError.text = "";
    }

    public void ClearPasswordError()
    {
        passwordError.text = "";
    }

}
