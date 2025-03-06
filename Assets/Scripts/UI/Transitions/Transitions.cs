using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using System;
using Unity.VisualScripting;
using System.Runtime.Serialization.Json;
using TMPro;

public class Transitions : MonoBehaviour
{
    private ARAnchorManager arAnchorManager;
    public List<GameObject> activeMenu = new List<GameObject>();
    public List<GameObject> mainMenuButtons = new List<GameObject>();
    private static Transitions instance;
    public bool optionsIsRotating = false; 
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Ensures persistence between scenes
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    public static Transitions Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {         
        arAnchorManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARAnchorManager>();         
    }


    public void NoTransitions(List<GameObject> buttons, bool reverse = false)
    {
       
        foreach (GameObject button in buttons)
        {
            button.SetActive(!reverse);

            if (!reverse)
            {
               /* ARAnchor anchor = button.GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    button.AddComponent<ARAnchor>();
                }*/
            }

        }
    }

    public IEnumerator SimplifiedTransitions(List<GameObject> buttons, bool reverse = false)
    {
        
        if (!reverse)
        {
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
                if (button.transform.childCount > 1)
                {     
                    List<Coroutine> fadeIns = new List<Coroutine>();
                    foreach (Transform child in button.transform)
                    {
                        fadeIns.Add(StartCoroutine(FadeIn(child.gameObject)));
                        
                    }

                    foreach (Coroutine coroutine in fadeIns)
                    { 
                     
                        yield return coroutine;
                    
                    }
                }
                else
                {
                    yield return FadeIn(button);
                }
              
                /*ARAnchor anchor = button.GetComponent<ARAnchor>();

                if (anchor == null)
                {
                    button.AddComponent<ARAnchor>();
                }*/

            }
        }
        else
        {
            for (int i = buttons.Count - 1; i >= 0; i--)
            {
                if (buttons[i].transform.childCount > 1)
                {   
                   List<Coroutine> fadeouts = new List<Coroutine>();
                    foreach (Transform child in buttons[i].transform)
                    {
                        fadeouts.Add(StartCoroutine(FadeOut(child.gameObject)));                    
                    }

                    foreach(Coroutine coroutine in fadeouts)
                    {
                        yield return coroutine;               
                    }
                }
                else
                {
                    yield return FadeOut(buttons[i]);
                }
                
                buttons[i].SetActive(false);
            }
        }
    }

    private IEnumerator FadeIn(GameObject button)
    {
        float fadeDuration = 0.5f;
        MeshRenderer buttonMeshRenderer = button.GetComponent<MeshRenderer>();

        if (buttonMeshRenderer != null)
        {
            Color buttonColor = buttonMeshRenderer.material.color;
            buttonColor.a = 0f;

            button.SetActive(true);

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Normalise to 0-1 range
                buttonColor.a = alpha;
                buttonMeshRenderer.material.color = buttonColor;
                yield return null;
            }

            buttonColor.a = 1f; 
            buttonMeshRenderer.material.color = buttonColor;
        }
    }

    private IEnumerator FadeOut(GameObject button)
    {
        float fadeDuration = 0.5f;
        MeshRenderer buttonMeshRenderer = button.GetComponent<MeshRenderer>();

        if (buttonMeshRenderer != null)
        {
            Color buttonColor = buttonMeshRenderer.material.color;
            buttonColor.a = 1f;

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); 
                buttonColor.a = alpha;
                buttonMeshRenderer.material.color = buttonColor;
                yield return null;
            }

            buttonColor.a = 0f; 
            buttonMeshRenderer.material.color = buttonColor;
            button.SetActive(false);
            buttonColor.a = 1f;
            buttonMeshRenderer.material.color = buttonColor;
        }
    }

    //Those two functions are from: https://discussions.unity.com/t/change-rendering-mode-via-script/667727/3 
    //Those basically do what happens behind the scenes when you change RenderMode on a material from the Inspector.
    private void ToOpaqueMode(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }

    private void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    public IEnumerator ComplexTransitions(List<GameObject> buttons, bool reverse = false)
    {

        Transform camera = Camera.main.transform;

        Vector3 cameraPosition = camera.position;
        if (!reverse)
        {
            for (int i = 0; i < buttons.Count; i++)
            {

                Transform initialButtonTransform = buttons[i].transform;
                buttons[i].transform.RotateAround(cameraPosition, new Vector3(0, 1, 0), 180);
                buttons[i].SetActive(true);
                float targetAngle = (i % 2 != 0) ? -180f : 180f;
                float currentAngle = 0f;
                float step = targetAngle > 0 ? 3f : -3f;

                while (Mathf.Abs(currentAngle) < Mathf.Abs(targetAngle))
                {
                    Rotation(cameraPosition, buttons[i], targetAngle < 0);
                    currentAngle += step;
                    yield return null;
                }
               
                buttons[i].transform.position = initialButtonTransform.position;
                buttons[i].transform.rotation = initialButtonTransform.rotation;
                
               /* ARAnchor anchor = buttons[i].GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    buttons[i].AddComponent<ARAnchor>();
                }*/
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                float targetAngle = (i % 2 != 0) ? -180f : 180f;
                float currentAngle = 0f;
             
                float step = targetAngle > 0 ? 3f : -3f;

                while (Mathf.Abs(currentAngle) < Mathf.Abs(targetAngle))
                {
  
                    Rotation(cameraPosition, buttons[i], targetAngle < 0);                  
                    currentAngle += step;

                    yield return null;
                }
          
                buttons[i].SetActive(false);
            }          
        }
    }

    private void Rotation(Vector3 cameraPosition,GameObject button,bool direction)
    {
        // true = comes from the left/goes to the right in front of the camera;
        // false = comes from the right/goes to the left in front of the camera;
        if (direction)
        {
            Vector3 relativePosition = cameraPosition - button.transform.position;
            relativePosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
            Quaternion current = button.transform.rotation;
            button.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            button.transform.RotateAround(cameraPosition, new Vector3(0, 1, 0), -3f);
        }
        else
        {
            Vector3 relativePosition = cameraPosition - button.transform.position;
            relativePosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
            Quaternion current = button.transform.rotation;
            button.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            button.transform.RotateAround(cameraPosition, new Vector3(0, 1, 0), 3f);
        }
       
    }

    private IEnumerator LeftRightArrowsTransition(Transform buttonBody, string direction)
    {
        int transitionType = Options.Instance().GetTransitionType();
        GameObject buttonName = null;

        foreach (Transform child in buttonBody)
        {
            if (child.CompareTag("buttonName"))
            {
                buttonName = child.gameObject;
                break; 
            }
        }

        Vector3 buttonNamePosition = buttonName.transform.position;
        Quaternion buttonNameRotation = buttonName.transform.rotation;
        string[] buttonTexts = { "None", "Simple", "Complex" };

        float elapsedTime = 0f;
        float targetAngle = 360f; 
        float rotationSpeed = 360f; 
        float rotationDuration = 1f;

        if (direction.Equals("leftArrow"))
        {   if(optionsIsRotating == false)
            {
                while (elapsedTime < rotationDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float progress = Mathf.Clamp01(elapsedTime / rotationDuration);
                    float currentAngle = progress * targetAngle;

                    buttonBody.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
                    buttonName.transform.rotation = buttonNameRotation;
                    buttonName.transform.position = buttonNamePosition;

                    if (progress >= 0.5f && progress < 0.51f) // Update text around 180 degrees
                    {
                        buttonBody.GetChild(1).GetComponent<TextMeshPro>().text = buttonTexts[transitionType];
                    }

                    yield return null;
                }
            }
          
        }
        else if (direction.Equals("rightArrow"))
        {   if(optionsIsRotating == false)
            {
                while (elapsedTime < rotationDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float progress = Mathf.Clamp01(elapsedTime / rotationDuration);
                    float currentAngle = progress * targetAngle;

                    buttonBody.transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime, Space.Self);
                    buttonName.transform.rotation = buttonNameRotation;
                    buttonName.transform.position = buttonNamePosition;

                    if (progress >= 0.5f && progress < 0.51f)
                    {
                        buttonBody.GetChild(1).GetComponent<TextMeshPro>().text = buttonTexts[transitionType];
                    }

                    yield return null;
                }
            }
        }

        optionsIsRotating = false;
    }

    public void LeftRightArrowsChangeOption(Transform arrow, string direction)
    {
        Transform buttonBody = null;
        Transform parentObjectTransform = arrow.parent;

        foreach (Transform child in parentObjectTransform)
        {
            if (child.childCount != 0)
            {
                buttonBody = child;          
            }
        }

        if (buttonBody != null) 
        { 
            StartCoroutine(Transitions.Instance().LeftRightArrowsTransition(buttonBody, direction)); 
        }
    }

    public void TransitionStarter(List<GameObject> buttons, bool reverse = false)
    {
        int transitionType = Options.Instance().GetTransitionType();

        if (transitionType == 2)
        {
            StartCoroutine(Transitions.Instance().ComplexTransitions(buttons, reverse));
        }
        else if (transitionType == 1)
        {
            StartCoroutine(Transitions.Instance().SimplifiedTransitions(buttons, reverse));
        }
        else
        {
            Transitions.Instance().NoTransitions(buttons, reverse);
        }

    }

    public void Repositioning(int transitionType)
    {

    }

}
