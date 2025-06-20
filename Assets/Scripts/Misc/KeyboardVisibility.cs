using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardVisibility : MonoBehaviour
{

    TMP_InputField inputField;
    private void Start()
    {
        inputField = this.gameObject.GetComponent<TMP_InputField>();
    }
    // Update is called once per frame
    void Update()
    { 
        if(TouchScreenKeyboard.visible == false)
        {
            Debug.Log("Chiar dispare keyboardVisibility");
            if (EventSystem.current != null && inputField != null && inputField.isFocused)
            {              
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("EventSystem.SetSelectedGameObject(null) apelat.");
            }
            else if (inputField != null && !inputField.isFocused)
            {               
                Debug.Log("InputField deja nu mai era focusat când tastatura a dispărut.");
            }                          
            this.gameObject.GetComponent<KeyboardVisibility>().enabled = false;
        }
      
    }
}
