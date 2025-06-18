using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditButton : MonoBehaviour
{
    [SerializeField]
    TMP_InputField editTitle, editContent;

    public void EnableEditing()
    {
        editTitle.enabled = true;
        editContent.enabled = true;
    }


}
