using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RemoveParent : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
       
        foreach (Transform child in this.transform)
        {
            child.SetParent(null);
        }
    }

   
}
