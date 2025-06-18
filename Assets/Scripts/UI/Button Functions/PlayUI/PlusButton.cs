using Google.XR.ARCoreExtensions.GeospatialCreator;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlusButton : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUI, noteCreation;
    private GameObject origin;

    void Start()
    {     
        origin = GameObject.FindGameObjectWithTag("origin");
    }

    public void SwitchUI()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(playUI));    
        StartCoroutine(Transitions.Instance().CanvasFadeIn(noteCreation));
        origin.GetComponent<PlaneSelection>().enabled = false;
    }

}
