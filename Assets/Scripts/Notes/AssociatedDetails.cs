using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssociatedDetails : MonoBehaviour
{
    private Note associatedNote;
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshPro title3D;

    public void SetNoteData(Note note)
    {
        associatedNote = note;
        if (title != null)
        {
            title.text = note.title;
        }
        if (title3D != null)
        {
            title3D.text = note.title;
        }
    }
    
    public Note GetAssociatedNote()
    {
        return associatedNote;
    }

    public TextMeshPro GetTitle3D()
    {
        return title3D;
    }
}
