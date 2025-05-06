using Lean.Touch;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EyePassword : MonoBehaviour
{
    private void OnEnable()
    {
        PasswordVisibility();
    }

    private void PasswordVisibility()
    {

        Transform passwordField = gameObject.transform.parent;
        TMP_InputField inputField = passwordField.GetComponent<TMP_InputField>();

        foreach (Transform child in passwordField)
        {

            if (child.childCount == 0)
            {
              
                if (child.gameObject.activeSelf)
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

            gameObject.GetComponent<EyePassword>().enabled = false;

        }


    }
}
