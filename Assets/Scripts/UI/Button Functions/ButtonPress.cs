using TMPro;
using UnityEngine;
using Lean.Touch;
using System;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class ButtonPress : MonoBehaviour
{
    private TextMeshPro buttonTextDebug;
    private void OnEnable()
    {
        LeanTouch.OnFingerTap += ButtonPressLogic;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= ButtonPressLogic;
    }

    private void ButtonPressLogic(LeanFinger finger)
    {
        Ray ray = finger.GetStartRay(Camera.main);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.collider.gameObject;
            if (objectHit == gameObject)
            {

                TextMeshPro buttonText = objectHit.GetComponentInChildren<TextMeshPro>();


                //Dau Enable la un script separat deoarece cand ma foloseam doar de ButtonPress event-ul OnFingerTap avea atatea subscribtii cate butoane aveau scriptul activ.
                if (buttonText != null)
                {
                    //if (Transitions.Instance().transitionEnded == true)
                    //{
                    if (buttonText.text.Equals("Play"))
                    {
                        GameObject origin = GameObject.FindGameObjectWithTag("origin");
                        origin.GetComponent<ARPlaneManager>().enabled = true;
                        origin.GetComponent<PlaneControl>().enabled = true;
                        origin.GetComponent<PlaneSelection>().enabled = true;
                        Transitions.Instance().TransitionStarter(Transitions.Instance().activeMenu, true);
                        Transitions.Instance().previousMenu = Transitions.Instance().mainMenuButtons;
                    }
                    else if (buttonText.text.Equals("Options"))
                    {

                        Transitions.Instance().TransitionStarter(Transitions.Instance().activeMenu, true);
                        Transitions.Instance().previousMenu = Transitions.Instance().mainMenuButtons;

                        GameObject optionsMenu = GameObject.FindGameObjectWithTag("options");
                        if (optionsMenu.GetComponent<OptionsMenu>().enabled)
                        {
                            OptionsMenu.delegateTrans();
                        }
                        else
                        {
                            optionsMenu.GetComponent<OptionsMenu>().enabled = true;
                        }

                    }
                    else if (buttonText.text.Equals("Credits"))
                    {
                        Transitions.Instance().TransitionStarter(Transitions.Instance().activeMenu, true);
                        Transitions.Instance().previousMenu = Transitions.Instance().mainMenuButtons;
                    }
                    else if (buttonText.text.Equals("Apply"))
                    {
                        Options.Instance().SaveOptions();
                    }
                    else if (buttonText.text.Equals("Default Settings"))
                    {
                        Options.Instance().ResetDefaultOptions();                   
                    }
                    //}
                }
                else
                {

                    if (objectHit.CompareTag("leftArrow"))
                    {
                        Options.Instance().ChangeTransitionType(false);
                        Transitions.Instance().LeftRightArrowsChangeOption(objectHit.transform, objectHit.gameObject.tag.ToString());
                        Transitions.Instance().optionsIsRotating = true;
                    }
                    else if (objectHit.CompareTag("rightArrow"))
                    {
                        Options.Instance().ChangeTransitionType(true);
                        Transitions.Instance().LeftRightArrowsChangeOption(objectHit.transform, objectHit.gameObject.tag.ToString());
                        Transitions.Instance().optionsIsRotating = true;
                    }
                    else if (objectHit.CompareTag("close"))
                    {
                        Transitions.Instance().TransitionStarter(Transitions.Instance().activeMenu, true);
                        Invoke("DelayMainMenuTransition", 2);
                        Transitions.Instance().activeMenu = Transitions.Instance().previousMenu;
                    }
                    else if(objectHit.CompareTag("password"))
                    {
                        objectHit.GetComponent<EyePassword>().enabled = true;
                    }
                }


            }

        }

    }

    private void DelayMainMenuTransition()
    {
        Transitions.Instance().TransitionStarter(Transitions.Instance().previousMenu, false);

    }

}
