using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MainMenu : MonoBehaviour
{
    private List<GameObject> mainMenuButtons;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButtons = new List<GameObject>();
    
        //hooking/subscribing to the stateChanged Event to check if the ARSession is Tracking. If I dont do this, the buttons show up before everything sets up.
        ARSession.stateChanged += ARSession_stateChanged;
        // Gets the immediate children's transform from the parents' Transform component
        
    }

    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {

        if (ARSession.state.Equals(ARSessionState.SessionTracking))
        {
            //unhooking because I need it once.
            ARSession.stateChanged -= ARSession_stateChanged;
            
            foreach (Transform button in transform)
            {
               
                mainMenuButtons.Add(button.gameObject);

            }
            Transitions.Instance().activeMenu = mainMenuButtons;
            Transitions.Instance().mainMenuButtons = mainMenuButtons;
            Transitions.Instance().TransitionStarter(mainMenuButtons);
        }
    }
}
