using TMPro;
using UnityEngine;
using Lean.Touch;
using Unity.XR.CoreUtils;



public class ButtonPress : MonoBehaviour
{
    private TextMeshPro buttonTextDebug;

    private void Start()
    {
        GameObject buttonTextGameObject = GameObject.FindGameObjectWithTag("debug");
        buttonTextDebug = buttonTextGameObject.GetComponent<TextMeshPro>();
    }

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
        //buttonTextDebug.text = "0";
        if(Physics.Raycast(ray, out hit))
        {
            //buttonTextDebug.text = "1";
            GameObject objectHit = hit.collider.gameObject;
            if(objectHit.CompareTag("button"))
            {
                //buttonTextDebug.text = "2";
                TextMeshPro buttonText = objectHit.GetComponentInChildren<TextMeshPro>();
                buttonText.text = "Pressed";
            }
        }

    }
}
