using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Transitions.Instance().TransitionStarter(Transitions.Instance().mainMenuButtons, true);
    }

   

}
