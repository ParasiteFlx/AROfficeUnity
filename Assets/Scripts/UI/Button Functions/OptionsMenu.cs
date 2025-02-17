using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private List<GameObject> optionsMenu = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {     
        foreach (Transform button in transform)
        {
            optionsMenu.Add(button.gameObject);
        }

        Invoke("Delay", 3);    
    }

    private void Delay()
    {
        Transitions.Instance().TransitionStarter(optionsMenu);
    }
}
