using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EyePassword : MonoBehaviour
{
    private void OnEnable()
    {
        LeanTouch.OnFingerTap += PasswordVisibility;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= PasswordVisibility;
    }

    


    private void PasswordVisibility(LeanFinger finger)
    {
        Ray ray = finger.GetStartRay(Camera.main);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.collider.gameObject;
            Transform passwordField = objectHit.transform.parent;
            TMP_InputField inputField = passwordField.GetComponent<TMP_InputField>();

            foreach(Transform child in passwordField)
            {
               
                if (child.childCount == 0)
                {   
                    if(child.gameObject.activeSelf)
                    {
                     
                        child.gameObject.SetActive(false);
                        inputField.contentType = TMP_InputField.ContentType.Password;
                        EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                        
                    }
                    else
                    {
                        
                        child.gameObject.SetActive(true);
                        inputField.contentType = TMP_InputField.ContentType.Standard;
                        EventSystem.current.SetSelectedGameObject(null);
                        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                    }
                }
            }
        }

    }
}
