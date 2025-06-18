using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CancelButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUI, noteCreation;
    [SerializeField]
    TMP_InputField title, content;
    private GameObject origin;

    private void Start()
    {
        origin = GameObject.FindGameObjectWithTag("origin");
    }

    public void Cancel()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(noteCreation));
        title.text ="";
        content.text = "";
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));       
        origin.GetComponent<PlaneSelection>().enabled = true;
    }
}
