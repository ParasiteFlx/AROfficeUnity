using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlaneControl : MonoBehaviour
{
    private ARPlaneManager ARPlaneManager;
    private ARRaycastManager ARRaycastManager;
    [SerializeField]
    private Camera ARCamera;
    private TextMeshPro buttonText;
    private List<ARPlane> disabledPlanes = new List<ARPlane>();
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private Boolean noActivePlanes;
    private int nrActivePlanes;
    private Dictionary<TrackableId, HashSet<TrackableId>> mergedPlanes = new Dictionary<TrackableId, HashSet<TrackableId>>();


    // Start is called before the first frame update
    // Intializes ARPlaneManager and ARRaycastManager, the main components of the script.
    void Start()
    {
        ARPlaneManager = gameObject.GetComponent<ARPlaneManager>();
        ARRaycastManager = gameObject.GetComponent<ARRaycastManager>();

        buttonText = GameObject.FindGameObjectWithTag("debug").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    // In Update we keep track of the number of activePlanes and we manage merged planes. It seems that a merged plane is a trackable and the planes that make it are trackables too,
    // so those still appear as active planes. Here we handle that. We also handle merged planes that merge into another larger plane.
    // Here I also enable ARPlaneManager as needed to avoid a clutter of indistinguishable planes that make for a bad user experience.
    // The raycast is shot here.
    // The accuracy of nrActivePlanes is vital for the script.
    void Update()
    {
        nrActivePlanes = 0;
        foreach (ARPlane plane in ARPlaneManager.trackables)
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

            if (ARPlaneManager.trackables.TryGetTrackable(key, out output))
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

        Vector2 screenCenter = new Vector2(ARCamera.pixelWidth / 2, ARCamera.pixelHeight / 2);

        raycastHits.Clear();
        //buttonText.text = "Inainte de Raycast";
        if (ARRaycastManager.Raycast(screenCenter, raycastHits, TrackableType.Planes))
        {
            // buttonText.text = "Dupa Raycast
            restorePlane(raycastHits);
            overlappedPlanes(raycastHits);
        }

        foreach (TrackableId key in mergedPlanes.Keys)
        {
           nrActivePlanes-=mergedPlanes[key].Count;

        }

        nrActivePlanes += mergedPlanes.Count;

        if (nrActivePlanes > 4)
        {
            ARPlaneManager.enabled = false;
        }
        else
        {
            ARPlaneManager.enabled = true;
        }

        int finalPlaneCount = 0;
        foreach (ARPlane plane in ARPlaneManager.trackables)
        {
            if (plane.gameObject.activeSelf && plane.trackingState == TrackingState.Tracking)
            {
                finalPlaneCount++;
            }
        }

        buttonText.text = ARPlaneManager.isActiveAndEnabled.ToString() + " " + nrActivePlanes.ToString() + " " + finalPlaneCount.ToString();

    }

    //The restorePlane function acts as a safety net. When some surfaces, that have been previously been disabled, need to reappear in an empty spot, with the raycast I enable
    //only the last disabled plane detected.

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

    //Manages most of the overlapping planes that appear due to some small height difference or due to some feature points appearing on patterns in the scene.
    //It separates planes between vertical and horizontal and with the help of a raycast shot from the screencenter disables all planes in, let's call it ray shot, except the last one.
    //To try not to disable planes on different levels of surfaces, I check for planes that have a distance smaller than 1f between them. (1f is commonly thought as 1 m, but I doubt it)
    //In the end it calls clearDisabledPlanes.

    private void overlappedPlanes(List<ARRaycastHit> raycastHits)
    {
        float overlapMaxDistance = 0.5f;

        if (raycastHits.Count > 1)
        {
            List<ARPlane> verticalPlanes = new List<ARPlane>();
            List<ARPlane> horizontalPlanes = new List<ARPlane>();

            for (int i = 0; i < raycastHits.Count; i++)
            {
                var plane = ARPlaneManager.GetPlane(raycastHits[i].trackableId);
                if (PlaneAlignmentExtensions.IsVertical(plane.alignment))
                {
                    verticalPlanes.Add(plane);
                }
                else if (PlaneAlignmentExtensions.IsHorizontal(plane.alignment))
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
                        if (Vector3.Distance(plane.center, verticalPlanes[verticalPlanes.Count - 1].center) < overlapMaxDistance)
                        {
                            if(plane.subsumedBy!=null)
                            {
                                mergedPlanes.Remove(plane.subsumedBy.trackableId);
                            }

                            plane.gameObject.SetActive(false);
                            disabledPlanes.Add(plane);
                          
                        }
                    }
                }
            }

            if (horizontalPlanes.Count > 1)
            {
                for (int i = 0; i < horizontalPlanes.Count - 1; i++)
                {
                    var plane = horizontalPlanes[i];
                    if (plane.gameObject.activeSelf && !disabledPlanes.Contains(plane))
                    {
                        if (Vector3.Distance(plane.center, horizontalPlanes[horizontalPlanes.Count - 1].center) < overlapMaxDistance)
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

            }

            clearDisabledPlanes(disabledPlanes);

        }

    }

    //Handles cleaning data and provides a good way to manage disabled planes that are still available for the ARPlaneManager

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