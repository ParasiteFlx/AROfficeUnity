using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;
using System;

public class Transitions : MonoBehaviour
{
    // 0 = noTransitions;
    // 1 = simplifiedTransitions:
    // 2 = complexTransitions;

    [SerializeField]
    private int transitionType;
    private ARAnchorManager arAnchorManager;
    public List<GameObject> mainMenuButtons = new List<GameObject>();
    private static Transitions instance;

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
        //arSession = GameObject.FindGameObjectWithTag("arSession").GetComponent<ARSession>();
        arAnchorManager = GameObject.FindGameObjectWithTag("origin").GetComponent<ARAnchorManager>();

        // Gets the immediate children's transform from the parents' Transform component
        foreach (Transform button in transform)
        {
            mainMenuButtons.Add(button.gameObject);
        }

        //hooking/subscribing to the stateChanged Event to check if the ARSession is Tracking. If I dont do this, the buttons show up before everything sets up.
        ARSession.stateChanged += ARSession_stateChanged;

    }

    private void ARSession_stateChanged(ARSessionStateChangedEventArgs obj)
    {
        
        if (ARSession.state.Equals(ARSessionState.SessionTracking))
        {
            //unhooking because I need it once.
            ARSession.stateChanged -= ARSession_stateChanged;
            TransitionStarter(mainMenuButtons);
        }
    }

    public int GetTransitionType()
    {
        return transitionType;
    }

    public void NoTransitions(List<GameObject> buttons, bool reverse = false)
    {
        Debug.Log(reverse);
        foreach (GameObject button in buttons)
        {
            button.SetActive(!reverse);

            if (!reverse)
            {
                ARAnchor anchor = button.GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    button.AddComponent<ARAnchor>();
                }
            }

        }
    }

    public IEnumerator SimplifiedTransitions(List<GameObject> buttons, bool reverse = false)
    {
        Debug.Log(reverse);
        if (!reverse)
        {
            foreach (GameObject button in buttons)
            {
                MeshRenderer buttonMeshRenderer = button.GetComponent<MeshRenderer>();
                Color buttonColor = buttonMeshRenderer.material.color;
                buttonColor.a = 0f;

                button.SetActive(true);

                for (float i = 0f; i <= 1f; i += 0.1f)
                {
                    buttonColor.a = i;
                    buttonMeshRenderer.material.color = buttonColor;
                    yield return new WaitForSeconds(0.05f);
                }

                buttonColor.a = 1f;
                buttonMeshRenderer.material.color = buttonColor;

                ARAnchor anchor = button.GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    button.AddComponent<ARAnchor>();
                }

            }
        }
        else
        {
            for (int i = mainMenuButtons.Count - 1; i >= 0; i--)
            {
                MeshRenderer buttonMeshRenderer = mainMenuButtons[i].GetComponent<MeshRenderer>();
                Color buttonColor = buttonMeshRenderer.material.color;

                for (float j = 1f; j >= 0f; j -= 0.1f)
                {
                    buttonColor.a = j;
                    buttonMeshRenderer.material.color = buttonColor;
                    yield return new WaitForSeconds(0.05f);
                }
                buttonColor.a = 0f;
                buttonMeshRenderer.material.color = buttonColor;
                mainMenuButtons[i].SetActive(false);

                ARAnchor anchor = mainMenuButtons[i].GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    mainMenuButtons[i].AddComponent<ARAnchor>();
                }
            }
        }
    }
    public IEnumerator ComplexTransitions(List<GameObject> buttons, bool reverse = false)
    {
        Debug.Log(reverse);
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
                float step = targetAngle > 0 ? 1f : -1f;

                while (Mathf.Abs(currentAngle) < Mathf.Abs(targetAngle))
                {
                    Rotation(cameraPosition, buttons[i], targetAngle < 0);
                    currentAngle += step;
                    yield return null;
                }
               
                buttons[i].transform.position = initialButtonTransform.position;
                buttons[i].transform.rotation = initialButtonTransform.rotation;
                
                ARAnchor anchor = mainMenuButtons[i].GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    mainMenuButtons[i].AddComponent<ARAnchor>();
                }
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                float targetAngle = (i % 2 != 0) ? -180f : 180f;
                float currentAngle = 0f;
             
                float step = targetAngle > 0 ? 1f : -1f;

                while (Mathf.Abs(currentAngle) < Mathf.Abs(targetAngle))
                {
  
                    Rotation(cameraPosition, buttons[i], targetAngle < 0);                  
                    currentAngle += step;

                    yield return null;
                }

                Debug.Log(currentAngle);
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
           // Vector3 relativePosition = cameraPosition - button.transform.position;
           // relativePosition.y = 0;
           // Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
           // Quaternion current = button.transform.rotation;
            //button.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            button.transform.RotateAround(cameraPosition, new Vector3(0, 1, 0), -1f);
        }
        else
        {
            //Vector3 relativePosition = cameraPosition - button.transform.position;
           // relativePosition.y = 0;
           // Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
           // Quaternion current = button.transform.rotation;
            //button.transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
            button.transform.RotateAround(cameraPosition, new Vector3(0, 1, 0), 1f);
        }
       
    }

    public void TransitionStarter(List<GameObject> buttons, bool reverse = false)
    {

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

}
