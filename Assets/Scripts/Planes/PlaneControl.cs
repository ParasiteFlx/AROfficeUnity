using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlaneControl : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    private ARRaycastManager arRaycastManager;
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private TextMeshPro debugText;
    private List<ARPlane> disabledPlanes = new List<ARPlane>();
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private Boolean noActivePlanes;
    private int nrActivePlanes;
    private Dictionary<TrackableId, HashSet<TrackableId>> mergedPlanes = new Dictionary<TrackableId, HashSet<TrackableId>>();


    // Start is called before the first frame update
    // Intializes ARPlaneManager and ARRaycastManager, the main components of the script.
    void Start()
    {
        arPlaneManager = gameObject.GetComponent<ARPlaneManager>();
        arRaycastManager = gameObject.GetComponent<ARRaycastManager>();
       
        
    }

    // Update is called once per frame
    // In Update I keep track of the number of activePlanes and I manage merged planes. It seems that a merged plane is a trackable and the planes that make it are trackables too,
    // so those still appear as active planes. Here I handle that. I also handle merged planes that merge into another larger plane.
    // Here I also enable ARPlaneManager as needed to avoid a clutter of indistinguishable planes that make for a bad user experience.
    // The raycast is shot here.
    // The accuracy of nrActivePlanes is vital for the script.
    void Update()
    {
        
        nrActivePlanes = 0;
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            if (plane.gameObject.activeSelf && plane.trackingState == TrackingState.Tracking)
            {
                ARPlane mergedPlane = plane.subsumedBy;

                if (mergedPlane != null)
                {
                    if (mergedPlanes.ContainsKey(mergedPlane.trackableId))
                    {   //Check for duplicates
                        if (!mergedPlanes[mergedPlane.trackableId].Contains(plane.trackableId))
                            mergedPlanes[mergedPlane.trackableId].Add(plane.trackableId);

                    }
                    else
                    {
                        HashSet<TrackableId> listTrackables = new HashSet<TrackableId>() { plane.trackableId };
                        mergedPlanes.Add(mergedPlane.trackableId, listTrackables);
                    }
                }
                else
                {
                    nrActivePlanes++;
                }
            }
        }

        ARPlane output;
        //when a merged plane merges
        List<TrackableId> keysToProcess = new List<TrackableId>(mergedPlanes.Keys);
        foreach (TrackableId key in keysToProcess)
        {

            if (arPlaneManager.trackables.TryGetTrackable(key, out output))
            {
                if (output.subsumedBy != null)
                {
                    ARPlane plane = output.subsumedBy;

                    foreach (var id in mergedPlanes[key])
                    {
                        mergedPlanes[plane.trackableId].Add(id);
                    }

                    mergedPlanes.Remove(key);

                }
            }
        }
        

        Vector2 screenCenter = new Vector2(arCamera.pixelWidth / 2, arCamera.pixelHeight / 2);

        raycastHits.Clear();
        //buttonText.text = "Inainte de Raycast";
        if (arRaycastManager.Raycast(screenCenter, raycastHits, TrackableType.Planes))
        {
          //  buttonText.text = "Dupa Raycast";
            RestorePlane(raycastHits);
            OverlappedPlanes(raycastHits);
        }

        foreach (TrackableId key in mergedPlanes.Keys)
        {
            nrActivePlanes -= mergedPlanes[key].Count;

        }

        nrActivePlanes += mergedPlanes.Count;

        if (nrActivePlanes > 3)
        {
            arPlaneManager.enabled = false;
        }
        else
        {
            arPlaneManager.enabled = true;
        }

        int finalPlaneCount = 0;
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            if (plane.gameObject.activeSelf && plane.trackingState == TrackingState.Tracking)
            {
                finalPlaneCount++;
            }
        }

        debugText.SetText(arPlaneManager.isActiveAndEnabled.ToString() + " " + nrActivePlanes.ToString() + " " + finalPlaneCount.ToString());

    }

    //The restorePlane function acts as a safety net. When some surfaces, that have been previously been disabled, need to reappear in an empty spot, with the raycast I enable
    //only the last disabled plane detected.

    private void RestorePlane(List<ARRaycastHit> raycastHits)
    {
        noActivePlanes = true;

        foreach (ARRaycastHit hit in raycastHits)
        {
            var plane = arPlaneManager.GetPlane(hit.trackableId);
            if (plane.gameObject.activeSelf)
            {
                noActivePlanes = false;
                break;
            }
        }

        if (noActivePlanes && raycastHits.Count > 0)
        {
            ARPlane biggestPlane = arPlaneManager.GetPlane(raycastHits[0].trackableId);
            float biggestPlaneSize = biggestPlane.size[0] * biggestPlane.size[1];

            foreach(ARRaycastHit hit in raycastHits)
            {
                ARPlane plane = arPlaneManager.GetPlane(hit.trackableId);
                float planeSize = plane.size[0] * plane.size[1];

                if (plane.size[0] * plane.size[1] >= biggestPlaneSize)
                {
                    biggestPlane = plane;
                    biggestPlaneSize = planeSize;
                }

            }

            biggestPlane.gameObject.SetActive(true);
            if (disabledPlanes.Contains(biggestPlane))
            {
                disabledPlanes.Remove(biggestPlane);
            }
        }
    }

    //Manages most of the overlapping planes that appear due to some small height difference or due to some feature points appearing on patterns in the scene.
    //It separates planes between vertical and horizontal and with the help of a raycast shot from the screencenter disables all planes in, let's call it ray shot, except the last one.
    //The planes are being disabled with the DisablePlanes function.
    //In the end it calls clearDisabledPlanes.

    private void OverlappedPlanes(List<ARRaycastHit> raycastHits)
    {
        if (raycastHits.Count > 1)
        {
            List<ARPlane> verticalPlanes = new List<ARPlane>();
            List<ARPlane> horizontalPlanes = new List<ARPlane>();

            for (int i = 0; i < raycastHits.Count; i++)
            {
                var plane = arPlaneManager.GetPlane(raycastHits[i].trackableId);
                if (PlaneAlignmentExtensions.IsVertical(plane.alignment))
                {
                    verticalPlanes.Add(plane);
                }
                else if (PlaneAlignmentExtensions.IsHorizontal(plane.alignment))
                {
                    horizontalPlanes.Add(plane);
                }
            }

            DisablePlanes(verticalPlanes);
            DisablePlanes(horizontalPlanes);
            ClearDisabledPlanes(disabledPlanes);

        }

    }

    //Disables Planes based on their size. Only the "biggest plane" remains active. 

    private void DisablePlanes(List<ARPlane> Planes)
    {
        if (Planes.Count > 1)
        {
            ARPlane biggestPlane = Planes[0];
            float biggestPlaneSize = Planes[0].size[0] * Planes[0].size[1];

            for (int i = 1; i < Planes.Count; i++)
            {
                var plane = Planes[i];
                float planeSize = plane.size[0] * plane.size[1];

                if (plane.gameObject.activeSelf)
                {               
                    if(planeSize >= biggestPlaneSize)
                    {                    
                        biggestPlane = plane;
                        biggestPlaneSize = planeSize;
                    }                  
                }

            }

            Planes.Remove(biggestPlane);

            foreach(var plane in Planes)
            {
                if (plane.subsumedBy != null)
                {
                    mergedPlanes.Remove(plane.subsumedBy.trackableId);
                }

                plane.gameObject.SetActive(false);
                disabledPlanes.Add(plane);

            }
        }
    }

    //Handles cleaning data and provides a good way to manage disabled planes that are still available for the ARPlaneManager

    private void ClearDisabledPlanes(List<ARPlane> disabledPlanes)
    {
        ARPlane output;
        List<ARPlane> copyList = new List<ARPlane>(disabledPlanes);
        foreach (ARPlane plane in copyList)
        {
            if (!arPlaneManager.trackables.TryGetTrackable(plane.trackableId, out output))
            {
                disabledPlanes.Remove(plane);

            }
        }

    }
}