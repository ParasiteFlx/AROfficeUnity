using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneButton : MonoBehaviour
{
    GameObject note3D;
    public delegate void SetNote3DDelegate(GameObject note3DInstance);
    public static SetNote3DDelegate setNote3DDeleg;

    private void Start()
    {
        setNote3DDeleg = SetNote3D;
    }

    public void SetNote3D(GameObject noteToBeSet)
    {
        note3D = noteToBeSet;
    }

}
