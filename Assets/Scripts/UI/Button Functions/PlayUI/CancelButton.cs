using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CancelButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUI, noteCreation,noteEdit;
    [SerializeField]
    TMP_InputField titleNoteCreation, contentNoteCreation, titleEditNote, contentEditNote;
    [SerializeField]
    Button editSaveButton;
    private GameObject origin;

    private void Start()
    {
        origin = GameObject.FindGameObjectWithTag("origin");
    }

    public void CancelNoteCreation()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(noteCreation));
        titleNoteCreation.text ="";
        contentNoteCreation.text = "";
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));       
        origin.GetComponent<PlaneSelection>().enabled = true;
    }

    public void CancelEditNote()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(noteEdit));
        titleEditNote.text = "";
        contentEditNote.text = "";
        titleEditNote.enabled = false;
        contentEditNote.enabled = false;
        editSaveButton.enabled = false;
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
        origin.GetComponent<PlaneSelection>().enabled = true;
    }
}
