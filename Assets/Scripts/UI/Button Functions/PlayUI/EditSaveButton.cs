using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditSaveButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUI, noteCreation;
    [SerializeField]
    private TMP_InputField editText, editContent;
    private List<Note> notesData;
    private int noteNumber;
    private GameObject origin;

    private void Start()
    {
        origin = GameObject.FindGameObjectWithTag("origin");      
    }

    public void SaveEdit()
    {
        notesData = Notes.getNotesListDeleg();
        noteNumber = EditNote.getNoteNumberDeleg();
        notesData[noteNumber].title = editText.text;
        notesData[noteNumber].content = editContent.text;
        Notes.setNoteListDeleg(notesData);
        GameObject currentNoteUI = EditNote.getCurrentNoteUI();
         
        if (currentNoteUI != null) {
            Debug.Log("Nu e null");
        }
        currentNoteUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = editText.text;
        EditNote.setCurrentNoteUI(currentNoteUI);      
    }

    public void Transition()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(noteCreation));  
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
        origin.GetComponent<PlaneSelection>().enabled = true;
    }
}
