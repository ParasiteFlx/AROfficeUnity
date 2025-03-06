using TMPro;
using UnityEngine;
using Lean.Touch;
using System;
using System.Collections;

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

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.collider.gameObject;        
            if(objectHit == gameObject)
            {
                
                TextMeshPro buttonText = objectHit.GetComponentInChildren<TextMeshPro>();
               
                //Dau Enable la un script separat deoarece cand ma foloseam doar de ButtonPress event-ul OnFingerTap avea atatea subscribtii cate butoane aveau scriptul activ.
                if (buttonText != null)
                {
                    Transitions.Instance().TransitionStarter(Transitions.Instance().activeMenu, true);
                    
                    if (buttonText.text.Equals("Play"))
                    {



                    }
                    else if (buttonText.text.Equals("Options"))
                    {

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


                    }
                }
                else
                {          
                   
                    if (objectHit.CompareTag("leftArrow"))
                    {
                        Options.Instance().SetTransitionType(false);
                        Transitions.Instance().LeftRightArrowsChangeOption(objectHit.transform,objectHit.gameObject.tag.ToString());                     
                        Transitions.Instance().optionsIsRotating = true; 
                    }
                    else if (objectHit.CompareTag("rightArrow"))
                    {
                        Options.Instance().SetTransitionType(true);
                        Transitions.Instance().LeftRightArrowsChangeOption(objectHit.transform, objectHit.gameObject.tag.ToString());
                        Transitions.Instance().optionsIsRotating = true;
                    }
                    else if(objectHit.CompareTag("close"))
                    {                                        
                        Transitions.Instance().TransitionStarter(Transitions.Instance().activeMenu, true);
                        Invoke("DelayMainMenuTransition", 3);
                        Transitions.Instance().activeMenu = Transitions.Instance().mainMenuButtons;
                    }
                }
            }
          

        }

    }

    private void DelayMainMenuTransition ()
    {
        Transitions.Instance().TransitionStarter(Transitions.Instance().mainMenuButtons, false);

    }
   
}
