using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private List<GameObject> optionsMenu = new List<GameObject>();
    public delegate void transitionDelegate();
    public static transitionDelegate delegateTrans;
    // Start is called before the first frame update
    void Start()
    {
        delegateTrans = TransitionStart;

        foreach (Transform button in transform)
        {
            optionsMenu.Add(button.gameObject);
        }

        delegateTrans();
    }

    private void TransitionStart()
    {
        Transitions.Instance().activeMenu = optionsMenu;
        Options.Instance().SetTemporaryTransitionType();
        Invoke("Delay", 2);
    }

    private void Delay()
    {
        Transitions.Instance().TransitionStarter(optionsMenu);
    }

}
