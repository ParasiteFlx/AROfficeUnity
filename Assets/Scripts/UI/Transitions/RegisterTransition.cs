using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterTransition : MonoBehaviour
{
    private GameObject loginForm, registerForm;
    private CanvasGroup loginCanvasGroup, registerCanvasGroup;
    void Start()
    {
        loginForm = gameObject.transform.GetChild(0).gameObject;
        loginCanvasGroup = loginForm.GetComponent<CanvasGroup>();
        registerForm = gameObject.transform.GetChild(1).gameObject;
        registerCanvasGroup = registerForm.GetComponent<CanvasGroup>();
    }

    public void FromLoginToRegister()
    {
        StartCoroutine(FromLoginToRegisterCoroutine());
    }

    private IEnumerator FromLoginToRegisterCoroutine()
    {
        yield return StartCoroutine(FadeOut(loginCanvasGroup));
        loginForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        registerForm.SetActive(true);
        yield return StartCoroutine(FadeIn(registerCanvasGroup));
    }
   
    private IEnumerator FadeOut(CanvasGroup target)
    {   
        float fadeDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            target.alpha = alpha;
            yield return null;
        }

        target.alpha = 0f;
    }

    private IEnumerator FadeIn(CanvasGroup target)
    {
        float fadeDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            target.alpha = alpha;
            yield return null;
        }

        target.alpha = 1f;
    }

}
