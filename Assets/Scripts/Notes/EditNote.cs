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
    private LinkedList<GameObject> listOfNotesUI;
    private List<Note> notesData;
    private int noteNumber;
    public delegate int GetNoteNumberDelegate();
    public static GetNoteNumberDelegate getNoteNumberDeleg;
    public delegate GameObject GetCurrentNoteUIDelegate();
    public static GetCurrentNoteUIDelegate getCurrentNoteUI;
    public delegate void SetCurrentNoteUIDelegat(GameObject newNoteUI);
    public static SetCurrentNoteUIDelegat setCurrentNoteUI;
    public delegate void SetListOfNotesUIDelegate();
    public static SetListOfNotesUIDelegate setListOfNotesUIdeleg;

    private void Start()
    {
        listOfNotesUI = new LinkedList<GameObject>(ListOfNotesUI.getListOfNotesDeleg());
        notesData = new List<Note>(Notes.getNotesListDeleg());
        origin = GameObject.FindGameObjectWithTag("origin");
        playUI = GameObject.FindGameObjectWithTag("play").GetComponent<CanvasGroup>();
        editNote = GameObject.FindGameObjectWithTag("editNote").GetComponent<CanvasGroup>();
        editTitle = GameObject.FindGameObjectWithTag("editTitle").GetComponent<TMP_InputField>();
        editContent = GameObject.FindGameObjectWithTag("editContent").GetComponent<TMP_InputField>();
        editTitle.enabled = false;
        editContent.enabled = false;
        this.GetComponent<Button>().onClick.AddListener(Transition);
        getNoteNumberDeleg = GetNoteNumber;
        getCurrentNoteUI = GetCurrentNoteUI;
        setCurrentNoteUI = SetCurrentNoteUI;           
    }

    private void SetDataInEdit()
    {
        notesData = new List<Note>(Notes.getNotesListDeleg());
        noteNumber = -1;
        foreach (GameObject noteUI in listOfNotesUI)
        {

            if (noteNumber < notesData.Count - 1)
            {
                noteNumber++;
            }

            if (noteUI.Equals(this.gameObject))
            {
                Debug.Log("SetDataInEdit");
                currentNoteUi = noteUI;
                editTitle.text = notesData[noteNumber].title;
                editContent.text = notesData[noteNumber].content;
            }         
        }
    }

    public int GetNoteNumber()
    {
        return noteNumber;
    }

    public GameObject GetCurrentNoteUI()
    {
        return currentNoteUi;
    }

    public void SetCurrentNoteUI(GameObject newNoteUI)
    {
        currentNoteUi = newNoteUI;
    }

    public void SetListOfNotesUI()
    {
        listOfNotesUI = new LinkedList<GameObject>(ListOfNotesUI.getListOfNotesDeleg());
    }

    public void Transition()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(playUI));
        SetDataInEdit();
        StartCoroutine(Transitions.Instance().CanvasFadeIn(editNote));
        origin.GetComponent<PlaneSelection>().enabled = false;
    }
}
