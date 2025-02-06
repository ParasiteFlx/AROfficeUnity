using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Transitions.Instance().TransitionStarter(Transitions.Instance().mainMenuButtons, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
