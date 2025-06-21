using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class NotePlacement : MonoBehaviour
{

    [SerializeField]
    private GameObject note3DPrefab;
    [SerializeField]
    private CanvasGroup notePlacementInfo, notePlacementControls;
    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;
    private GameObject note3D;

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

                note3D = Instantiate(note3DPrefab);
                float offsetDistance = 0.01f; 
                note3D.transform.position = selectedPlane.center + (selectedPlane.transform.up * offsetDistance);
                // Debug.Log("3DNote Original : " + note3D.transform.rotation + " " + note3D.transform.forward + " " + note3D.transform.up + " " + note3D.transform.eulerAngles);
                note3D.transform.rotation = Quaternion.LookRotation(selectedPlane.transform.up, Vector3.up);
                // Debug.Log("Selected Plane : " + selectedPlane.transform.rotation + " " + selectedPlane.transform.forward + " " + selectedPlane.transform.up + " " + selectedPlane.transform.eulerAngles);
                //Debug.Log("3DNote dupa schimbare : " + note3D.transform.rotation + " " + note3D.transform.forward + " " + note3D.transform.up + " " + note3D.transform.eulerAngles);
                note3D.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
             
                UpButton.setNote3DDeleg(note3D);          
                DownButton.setNote3DDeleg(note3D);         
                LeftButton.setNote3DDeleg(note3D);    
                RightButton.setNote3DDeleg(note3D);    
                DoneButton.setNote3DDeleg(note3D);
                
                StartCoroutine(Transitions.Instance().CanvasFadeIn(notePlacementControls));
                this.gameObject.GetComponent<NotePlacement>().enabled = false;
            }
        }
    }
}

