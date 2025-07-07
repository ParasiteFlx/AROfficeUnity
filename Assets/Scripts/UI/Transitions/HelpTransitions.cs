using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTransitions : MonoBehaviour
{
    [SerializeField]
    CanvasGroup playUI, help;

    public void FadeInHelp()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(playUI));
        StartCoroutine(Transitions.Instance().CanvasFadeIn(help));
    }

    public void FadeOutHelp()
    {
        StartCoroutine(Transitions.Instance().CanvasFadeOut(help));
        StartCoroutine(Transitions.Instance().CanvasFadeIn(playUI));
    }
}
