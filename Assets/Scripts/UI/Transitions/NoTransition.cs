using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class NoTransition : MonoBehaviour
{
    private ARAnchorManager arAnchorManager;
    private List<GameObject> mainMenuButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        arAnchorManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARAnchorManager>();

        // Gets the immediate children's transform from the parents' Transform component
        foreach(Transform button in transform)
        {
            mainMenuButtons.Add(button.gameObject);
        }

        ButtonsSetup(mainMenuButtons);
    }

    private void ButtonsSetup(List<GameObject> buttons)
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

}
