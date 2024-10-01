using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class NoTransition : MonoBehaviour
{
    [SerializeField]
    private bool noTransitions;
    [SerializeField]
    private bool simplifiedTransitions;
    [SerializeField]
    private bool complexTransitions;
    //private ARSession arSession;
    private ARAnchorManager arAnchorManager;
    private List<GameObject> mainMenuButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {   
        //arSession = GameObject.FindGameObjectWithTag("arSession").GetComponent<ARSession>();
        arAnchorManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARAnchorManager>();
        
        // Gets the immediate children's transform from the parents' Transform component
        foreach(Transform button in transform)
        {
            mainMenuButtons.Add(button.gameObject);
        }

        //hooking/subscribing to the stateChanged Event to check if the ARSession is Tracking. If I dont do this, the buttons show up before everything sets up.
        ARSession.stateChanged += ARSession_stateChanged;
        
    }

    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {
        if (ARSession.state.Equals(ARSessionState.SessionTracking))
        {   
            //unhooking because I need it once.
            ARSession.stateChanged-= ARSession_stateChanged;

            if (noTransitions)
            {
                NoTransitions(mainMenuButtons);
            }
            else if (simplifiedTransitions)
            {

                StartCoroutine(SimplifiedTransitions(mainMenuButtons));
            }
            else if (complexTransitions)
            {

            }
        }

    }

    private void NoTransitions(List<GameObject> buttons)
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(true);
            ARAnchor anchor = button.GetComponent<ARAnchor>();            
            if(anchor == null)
            { 
               button.AddComponent<ARAnchor>();              
            }
        }
    }

    private IEnumerator SimplifiedTransitions(List<GameObject> buttons)
    {
        foreach(GameObject button in buttons)
        {
            MeshRenderer buttonMeshRenderer = button.GetComponent<MeshRenderer>();
            Color buttonColor = buttonMeshRenderer.material.color;

            buttonColor.a = 0f;

            button.SetActive(true);       

            for(float i = 0f; i <= 1f; i+=0.1f)
            {
                buttonColor.a = i;
                buttonMeshRenderer.material.color = buttonColor;
                yield return new WaitForSeconds(0.1f);
            }

            buttonColor.a = 1f;
            buttonMeshRenderer.material.color = buttonColor;

            ARAnchor anchor = button.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                button.AddComponent<ARAnchor>();
            }

           
        }
    }

}
