using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    private GameObject origin;
    [SerializeField]
    private GameObject plusButton;
    [SerializeField]
    private CanvasGroup playUI, noteCreation, notePlacement;
    [SerializeField]
    private TMP_InputField title, content;

    void Start()
    {
        origin = GameObject.FindGameObjectWithTag("origin");
    }

    public void AddNote()
    {
        transitionOut();
        Note tempNote = Notes.getTempNoteDeleg();
        tempNote.title = title.text;
        tempNote.content = content.text;
        Debug.Log(tempNote.title + " si " + tempNote.content);
        Notes.setTempNoteDeleg(tempNote);
        Notes.addNoteDeleg(tempNote);
        title.text = "";
        content.text = "";
    }

    private void transitionOut()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(noteCreation));     
        StartCoroutine(Transitions.Instance().CanvasFadeIn(notePlacement));
        origin.GetComponent<PlaneSelection>().enabled = true;
    }
    
}
