using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditSaveButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUI, noteCreation;
    [SerializeField]
    private TMP_InputField editText, editContent;
    private List<Note> notesData;
    private int noteNumber;
    private GameObject origin, currentNoteUI;

    private void Start()
    {
        origin = GameObject.FindGameObjectWithTag("origin");      
    }

    public void SaveEdit()
    {  
        notesData = Notes.getNotesListDeleg();
        notesData[noteNumber].title = editText.text;
        notesData[noteNumber].content = editContent.text;
        Notes.setNoteListDeleg(notesData);            
        currentNoteUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = editText.text;
        currentNoteUI.GetComponent<AssociatedDetails>().SetNoteData(notesData[noteNumber]);
        editText.enabled = false;
        editContent.enabled = false;
    }

    public void setCurrentNoteUI(GameObject currentNoteUI)
    {
        this.currentNoteUI = currentNoteUI;
    }

    public void setNoteNumber(int noteNumber)
    {
        this.noteNumber = noteNumber;
    }

    public void Transition()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(noteCreation));  
        this.gameObject.GetComponent<Button>().interactable = false;
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
        origin.GetComponent<PlaneSelection>().enabled = true;
    }
}
