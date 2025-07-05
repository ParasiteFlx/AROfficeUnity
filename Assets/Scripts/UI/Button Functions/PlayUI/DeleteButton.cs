using System;
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
        Debug.Log("ajung 0 ");
        noteToBeRemoved = EditNote.getCurrentNoteUiDeleg();
         Debug.Log("ajung 1 ");
        AssociatedDetails noteTobeRemovedDetails = noteToBeRemoved.GetComponent<AssociatedDetails>();
        Debug.Log("ajung 2 ");
        string noteToBeRemovedID = noteTobeRemovedDetails.GetAssociatedNote().id;     
        Debug.Log("ajung 3");
        GameObject[] notes3D = GameObject.FindGameObjectsWithTag("note3D");
        if (notes3D.Length == 0)
        {
            Debug.Log("E notes3D null");
        }
        else
        {
            foreach (GameObject note in notes3D)
            {
                if (note != null)
                {
                    AssociatedDetails note3DDetails = note.GetComponent<AssociatedDetails>();
                    Note note3DAssociatedNote = note3DDetails.GetAssociatedNote();
                    if (note3DAssociatedNote == null)
                    {
                        AndroidToast.sendToast("This operation is not available. Try again in a few seconds!");
                    }
                    else
                    {                        
                            string noteId = note3DDetails.GetAssociatedNote().id;
                            if (noteToBeRemovedID.Equals(noteId))
                            {                               
                                Destroy(note);
                            }
                            Notes.removeNoteDeleg(noteTobeRemovedDetails.GetAssociatedNote());
                            ListOfNotesUI.removeNoteDeleg(noteToBeRemoved);
                    }

                }
                else
                {
                    Debug.Log("E note null!");
                }

            }
        }

        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
     }
}
