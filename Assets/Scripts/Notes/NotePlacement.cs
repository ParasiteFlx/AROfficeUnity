using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class NotePlacement : MonoBehaviour
{

    [SerializeField]
    private GameObject note3D;
    [SerializeField]
    private CanvasGroup notePlacementInfo;
    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;

    private void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += InstantiateNote;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= InstantiateNote;
    }


    private void InstantiateNote(LeanFinger finger)
    {

        Ray ray = finger.GetStartRay(Camera.main);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit) )
        {
            if (hit.collider.gameObject.CompareTag("ARPlane"))
            {
                StartCoroutine(Transitions.Instance().CanvasFadeOut(notePlacementInfo));
                LeanTouch.OnFingerTap -= InstantiateNote;

                ARPlane selectedPlane = hit.collider.gameObject.GetComponent<ARPlane>();

                Instantiate(note3D, selectedPlane.center, new Quaternion(0,0,0,0));
               
            }
        }
    }
}

