using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUITransition : MonoBehaviour
{
    private CanvasGroup playUICanvas;

    // Start is called before the first frame update
    void Start()
    {
        playUICanvas = gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUICanvas));      
    }

}
