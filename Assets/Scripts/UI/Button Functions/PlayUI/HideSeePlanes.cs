using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HideSeePlanes : MonoBehaviour
{
    [SerializeField]
    GameObject hidePlanes, seePlanes;
    GameObject origin;
    ARPlaneManager planeManager; 

    private void Start()
    {
        origin = GameObject.FindGameObjectWithTag("origin");
        planeManager = origin.GetComponent<ARPlaneManager>();
    }

    public void HidePlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            if (plane.isActiveAndEnabled)
            {
               
                plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
                
                origin.GetComponent<PlaneSelection>().enabled = false;
            }
        }
        Debug.Log("Null e inainte de hide");
        hidePlanes.SetActive(false);
        Debug.Log("Null e dupa de hide");
        seePlanes.SetActive(true);
    }
    
    public void SeePlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            if (plane.isActiveAndEnabled)
            {
                plane.GetComponent<ARPlaneMeshVisualizer>().enabled = true;
                origin.GetComponent<PlaneSelection>().enabled = true;
            }
        }
        hidePlanes.SetActive(true);
        seePlanes.SetActive(false);
    }

}
