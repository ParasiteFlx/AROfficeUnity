using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShareTransitions : MonoBehaviour
{

    [SerializeField]

    CanvasGroup playUI, shareInterface;

    [SerializeField]
    TMP_InputField userID;

    public void FromPlayToShare()
    {
       StartCoroutine(Transitions.Instance().CanvasFadeOut(playUI));
       StartCoroutine(Transitions.Instance().CanvasFadeIn(shareInterface));
    }

    public void FromShareToPlay()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(shareInterface));
        userID.text = "";
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
    }
}
