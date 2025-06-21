using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class UpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject note3D;
    private float speed;
    public delegate void SetNote3DDelegate(GameObject note3DInstance);
    public static SetNote3DDelegate setNote3DDeleg;
    private bool isButtonPressed;

    private void Start()
    {      
        speed = 1f;
        setNote3DDeleg = SetNote3D;
    }
    private void Update()
    {
        if (isButtonPressed && note3D != null) 
        {
            note3D.transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
    }

    public void SetNote3D(GameObject note3DInstance)
    {
        note3D = note3DInstance;
    }
}
