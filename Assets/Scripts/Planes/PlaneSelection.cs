using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Lean.Touch;

public class PlaneSelection : MonoBehaviour
{

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private ARPlaneManager arPlaneManager;
    private GameObject previousPlane;
    private GameObject selectedPlane;

    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        /*GameObject buttonTextGameObject = GameObject.FindGameObjectWithTag("debug");
        buttonText = buttonTextGameObject.GetComponent<TextMeshPro>();
       */
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += AlternativeSelection;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= AlternativeSelection;
    }

    private void AlternativeSelection(LeanFinger finger)
    {
        
        Ray ray = finger.GetStartRay(Camera.main);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (previousPlane != null && previousPlane.gameObject.activeSelf == true && hit.collider.gameObject.CompareTag("ARPlane"))
            {
                previousPlane.GetComponent<LineRenderer>().startWidth = 0.005f;
                previousPlane.GetComponent<LineRenderer>().endWidth = 0.005f;
            }


            if(selectedPlane !=null && hit.collider.gameObject.Equals(selectedPlane))
            {
                selectedPlane.GetComponent<LineRenderer>().startWidth = 0.005f;
                selectedPlane.GetComponent<LineRenderer>().endWidth = 0.005f;
                selectedPlane = null;
            }
            else
            {
                selectedPlane = hit.collider.gameObject;
                selectedPlane.GetComponent<LineRenderer>().startWidth = 0.03f;
                selectedPlane.GetComponent<LineRenderer>().endWidth = 0.03f;
                previousPlane = selectedPlane;
            }
      
        }
    }
}

