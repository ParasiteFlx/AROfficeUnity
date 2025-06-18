using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditButton : MonoBehaviour
{
    [SerializeField]
    TMP_InputField editTitle, editContent;
    [SerializeField]
    Button saveButton;

    public void EnableEditing()
    {
        editTitle.enabled = true;
        editContent.enabled = true;
        saveButton.interactable = true;
    }


}
