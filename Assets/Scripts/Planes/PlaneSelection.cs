using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class PlaneSelection : MonoBehaviour
{

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private ARPlaneManager arPlaneManager;
    private ARPlane previousPlane;
    private TextMeshPro buttonText;
    private Touch touch;
    private int counterPlaneCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        GameObject buttonTextGameObject = GameObject.FindGameObjectWithTag("debug");
        buttonText = buttonTextGameObject.GetComponent<TextMeshPro>();

    }

    // Update is called once per frame
    void Update()
    {
        PlaneSelectionLogic();
    }

    private void PlaneSelectionLogic()
    {

       

        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            //buttonText.text = touch.position.ToString();

            if (arRaycastManager.Raycast(touch.position, raycastHits, TrackableType.Planes))
            {

                if (previousPlane != null && previousPlane.gameObject.activeSelf == true)
                {
                    previousPlane.GetComponent<LineRenderer>().startWidth = 0.005f;
                    previousPlane.GetComponent<LineRenderer>().endWidth = 0.005f;
                }

               Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.y));

                Dictionary<float, ARPlane> planesAndDistancesV1 = new Dictionary<float, ARPlane>();
                Dictionary<float, ARPlane> planesAndDistancesV2 = new Dictionary<float, ARPlane>();
                List<float> sortedDistancesV1 = new List<float>();
                List<float> sortedDistancesV2 = new List<float>();
                // Varianta 1
                // Here I determine the most likely plane that has been selected by finding the plane with the smallest distance from the worldTouchPosition to
                // the raycast's interesection with the plane. This alone is quite accurate but somehow leads to visual glitches.
           
                ARPlane selectedPlane = null;

                foreach (ARRaycastHit hit in raycastHits)
                {
                    ARPlane plane = arPlaneManager.GetPlane(hit.trackableId);

                    if (plane.gameObject.activeSelf)
                    {
                        float distance = Vector3.Distance(hit.pose.position, worldTouchPosition);
                        planesAndDistancesV1.Add(distance, plane);
                        sortedDistancesV1.Add(distance);
                    }
                }

                sortedDistancesV1.Sort();
                selectedPlane = planesAndDistancesV1[sortedDistancesV1[0]];


                //Varianta 2
                //Here I use a different method. I sort the planes by the distances from their center in world space to the worldTouchPosition and afterwards,
                //using the sorted list of the distances I go through each plane and calculate mean distance between their center in plane space and the boundary points.
                //If the distance from the world space center of the plane to the worldTouchPosition is smaller than the meanDistance, then there is a chance that the
                //touch was indeed inside that plane. This method is less accurate than the first one, but does not produce significant visual glitches.

                foreach (ARRaycastHit hit in raycastHits)
                    {
                        ARPlane plane = arPlaneManager.GetPlane(hit.trackableId);

                        if (plane.gameObject.activeSelf)
                        {
                            float distance = Vector3.Distance(plane.transform.position, worldTouchPosition);
                            planesAndDistancesV2.Add(distance, plane);
                            sortedDistancesV2.Add(distance);
                        }
                    }

                    sortedDistancesV2.Sort();

                    foreach (float minDistance in sortedDistancesV2)
                    {   //counter as in a response.
                        ARPlane counterPlane = planesAndDistancesV2[minDistance];
                        NativeArray<Vector2> planeBoundaryPoints = counterPlane.boundary;
                        float sum = 0;
                        foreach (Vector2 point in planeBoundaryPoints)
                        {
                            sum += Vector2.Distance(point, counterPlane.centerInPlaneSpace);
                        }
                        float meanDistance = sum / planeBoundaryPoints.Count();

                        if (minDistance < meanDistance)
                        {
                            //If counterPlane and selectedPlane are one and the same, then that is the intended selected plane.
                            //If they are not the same I choose the plane with the smaller distance. Sure this decision is arbitrary and finite, but it lets me avoid
                            //various pitfalls and falls leads that would be caused by a ping-pong approach, and by that I mean searching one by one, and slightly changing the candidates

                            if (selectedPlane.trackableId != counterPlane.trackableId)
                            {
                                if (minDistance < sortedDistancesV1[0])
                                {
                                    selectedPlane = counterPlane;
                                   
                                    counterPlaneCounter++;

                                }
                            }

                            break;
                        }
                    }
                    //buttonText.text = counterPlaneCounter.ToString();
                    selectedPlane.GetComponent<LineRenderer>().startWidth = 0.03f;
                    selectedPlane.GetComponent<LineRenderer>().endWidth = 0.03f;
                    previousPlane = selectedPlane;
                
            }

            raycastHits.Clear();

        }
    }
}

