using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EditNote : MonoBehaviour
{
    private CanvasGroup playUI, editNote;
    private TMP_InputField editTitle, editContent;
    private GameObject origin, currentNoteUi;
    private List<Note> notesData;
    private int noteNumber;

    private void Start()
    {       
        notesData = Notes.getNotesListDeleg();
        origin = GameObject.FindGameObjectWithTag("origin");
        playUI = GameObject.FindGameObjectWithTag("play").GetComponent<CanvasGroup>();
        editNote = GameObject.FindGameObjectWithTag("editNote").GetComponent<CanvasGroup>();
        editTitle = GameObject.FindGameObjectWithTag("editTitle").GetComponent<TMP_InputField>();
        editContent = GameObject.FindGameObjectWithTag("editContent").GetComponent<TMP_InputField>();
        editTitle.enabled = false;
        editContent.enabled = false;
        this.GetComponent<Button>().onClick.AddListener(Transition);
            
    }

    private void SetDataInEdit()
    {
       
        currentNoteUi = this.gameObject;
      
        Debug.Log($"[EditNote] SetDataInEdit called. 'this.gameObject' (the clicked UI) is: {this.gameObject.name} (Instance ID: {this.gameObject.GetInstanceID()})");
        Debug.Log($"[EditNote] 'currentNoteUi' field now holds: {currentNoteUi.name} (Instance ID: {currentNoteUi.GetInstanceID()})");

        noteNumber = -1;
        AssociatedDetails associatedDetails = this.gameObject.GetComponent<AssociatedDetails>();
        Note associatedNote = associatedDetails.GetAssociatedNote();
        string targetNoteId = associatedNote.id;
        editTitle.text = associatedNote.title;
        editContent.text = associatedNote.content;
        notesData = Notes.getNotesListDeleg();
        for (int i = 0; i < notesData.Count; i++)
        {          
            Note currentNoteInList = notesData[i];
       
            if (currentNoteInList.id == targetNoteId)
            {             
                noteNumber = i;
                break; 
            }
        }
    }

    public void SetCurrentNoteUI(GameObject newNoteUI)
    {
        currentNoteUi = newNoteUI;
    }

    public void Transition()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(playUI));
        SetDataInEdit();
        EditSaveButton saveButton = Transitions.Instance().GetEditSaveButton();
        saveButton.setCurrentNoteUI(currentNoteUi);
        saveButton.setNoteNumber(noteNumber);
        StartCoroutine(Transitions.Instance().CanvasFadeIn(editNote));
        origin.GetComponent<PlaneSelection>().enabled = false;
    }
}
