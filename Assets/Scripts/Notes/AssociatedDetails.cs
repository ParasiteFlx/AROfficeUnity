using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssociatedDetails : MonoBehaviour
{
    private Note associatedNote;
    [SerializeField]
    TextMeshProUGUI title;

    public void SetNoteData(Note note)
    {
        associatedNote = note;
        if (title != null)
        {
            title.text = note.title;
        }
    }
    
    public Note GetAssociatedNote()
    {
        return associatedNote;
    }
}
