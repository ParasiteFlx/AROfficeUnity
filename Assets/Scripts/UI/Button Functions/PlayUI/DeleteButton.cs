using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{

    [SerializeField]
    CanvasGroup playUI, editNote;
    GameObject noteToBeRemoved;
    
    public void Delete()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(editNote));
        noteToBeRemoved = EditNote.getCurrentNoteUiDeleg();
        AssociatedDetails noteTobeRemovedDetails = noteToBeRemoved.GetComponent<AssociatedDetails>();
        Notes.removeNoteDeleg(noteTobeRemovedDetails.GetAssociatedNote());
        ListOfNotesUI.removeNoteDeleg(noteToBeRemoved);
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
     }
}
