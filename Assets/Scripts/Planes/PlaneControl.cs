using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneControl : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager ARPlaneManager;
    [SerializeField]
    private ARRaycastManager ARRaycastManager;
    [SerializeField]
    private Camera ARCamera;
    //[SerializeField]
    //private TextMeshPro buttonText;
    private List<ARPlane> disabledPlanes = new List<ARPlane>();
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private Boolean noActivePlanes;

    // Start is called before the first frame update
    void Start()
    {
        if (ARPlaneManager == null)
        {
            ARPlaneManager = GetComponent<ARPlaneManager>();
        }
        if (ARRaycastManager == null)
        {
            ARRaycastManager = GetComponent<ARRaycastManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ARPlaneManager.trackables.count - disabledPlanes.Count > 4)
        {
            ARPlaneManager.enabled = false;
            //buttonText.text = ARPlaneManager.isActiveAndEnabled.ToString() +" "+ ARPlaneManager.trackables.count.ToString() + " " + disabledPlanes.Count.ToString();
        }
        else
        {
            ARPlaneManager.enabled = true;
            //buttonText.text = ARPlaneManager.isActiveAndEnabled.ToString() + " " + ARPlaneManager.trackables.count.ToString() + " " + disabledPlanes.Count.ToString();
        }

        Vector2 screenCenter = new Vector2(ARCamera.pixelWidth / 2, ARCamera.pixelHeight / 2);

        raycastHits.Clear();
        //buttonText.text = "Inainte de Raycast";
        if (ARRaycastManager.Raycast(screenCenter, raycastHits, TrackableType.Planes))
        {
            // buttonText.text = "Dupa Raycast
            restorePlane(raycastHits);
            overlappedPlanes(raycastHits);
        }

    }

    private void restorePlane(List<ARRaycastHit> raycastHits)
    {
        noActivePlanes = true;

        foreach (ARRaycastHit hit in raycastHits)
        {
            var plane = ARPlaneManager.GetPlane(hit.trackableId);
            if (plane.gameObject.activeSelf)
            {
                noActivePlanes = false;
                break;
            }
        }

        if (noActivePlanes && raycastHits.Count > 0)
        {
            var plane = ARPlaneManager.GetPlane(raycastHits[raycastHits.Count - 1].trackableId);
            plane.gameObject.SetActive(true);
            if (disabledPlanes.Contains(plane))
            {
                disabledPlanes.Remove(plane);
            }
        }
    }

    private void overlappedPlanes(List<ARRaycastHit> raycastHits)
    {

        if (raycastHits.Count > 1)
        {
          List<ARPlane> verticalPlanes = new List<ARPlane>();
          List<ARPlane> horizontalPlanes = new List<ARPlane>(); 

           for (int i = 0; i < raycastHits.Count; i++)
            {
                var plane = ARPlaneManager.GetPlane(raycastHits[i].trackableId);
                if(PlaneAlignmentExtensions.IsVertical(plane.alignment))
                {
                    verticalPlanes.Add(plane);
                }
                else if(PlaneAlignmentExtensions.IsHorizontal(plane.alignment))
                {
                    horizontalPlanes.Add(plane);
                }
            }

            if (verticalPlanes.Count > 1)
            {
                for (int i = 0; i < verticalPlanes.Count - 1; i++)
                {
                    var plane = verticalPlanes[i];
                    if (plane.gameObject.activeSelf && !disabledPlanes.Contains(plane))
                    {
                        plane.gameObject.SetActive(false);
                        disabledPlanes.Add(plane);
                    }
                }
            }

            if(horizontalPlanes.Count > 1)
            {
                for (int i = 0; i < horizontalPlanes.Count - 1; i++)
                {
                    var plane = horizontalPlanes[i];
                    if (plane.gameObject.activeSelf && !disabledPlanes.Contains(plane))
                    {
                        plane.gameObject.SetActive(false);
                        disabledPlanes.Add(plane);
                    }
                }

            }

            clearDisabledPlanes(disabledPlanes);

        }

    }

    private void clearDisabledPlanes(List<ARPlane> disabledPlanes)
    {
        ARPlane output;
        List<ARPlane> copyList = new List<ARPlane>(disabledPlanes);
        foreach (ARPlane plane in copyList)
        {
            if (!ARPlaneManager.trackables.TryGetTrackable(plane.trackableId, out output))
            {
                disabledPlanes.Remove(plane);
            }
        }
    }

}