using TMPro;
using UnityEngine;
using Lean.Touch;

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

            //Dau Enable la un script separat deoarece cand ma foloseam doar de ButtonPress event-ul OnFingerTap avea atatea subscribtii cate butoane aveau scriptul activ.
            if(objectHit.CompareTag("play"))
            {
                objectHit.GetComponent<PlayButton>().enabled = true;
                Debug.Log(objectHit.tag);

            }
            else if(objectHit.CompareTag("options"))
            {
                objectHit.GetComponent<OptionsButton>().enabled = true;
                Debug.Log(objectHit.tag);

            }
            else if(objectHit.CompareTag("credits"))
            {
                objectHit.GetComponent<CreditsButton>().enabled = true;
                Debug.Log(objectHit.tag);

            }
        }

    }
   
}
