using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterTransition : MonoBehaviour
{
    private GameObject loginForm, registerForm, menu;
    private CanvasGroup loginCanvasGroup, registerCanvasGroup;
    public delegate void RegisterTransitionDelegate();
    public static RegisterTransitionDelegate fromLogToReg;
    public delegate void LoginMainMenuDelegate();
    public static LoginMainMenuDelegate fromLogToMainMenu;
    [SerializeField]
    private TMP_InputField usernameInput, passwordInput, confirmPass, email, loginUsernameInput, loginPasswordInput;

    void Start()
    {
        fromLogToReg = FromRegisterToLogin;
        fromLogToMainMenu = FromLoginToMainMenu;
        loginForm = gameObject.transform.GetChild(0).gameObject;
        loginCanvasGroup = loginForm.GetComponent<CanvasGroup>();
        registerForm = gameObject.transform.GetChild(1).gameObject;
        registerCanvasGroup = registerForm.GetComponent<CanvasGroup>();
        menu = GameObject.FindGameObjectWithTag("menu");
    }

    public void FromLoginToRegister()
    {
        StartCoroutine(FromLoginToRegisterCoroutine());
    }

    public void FromRegisterToLogin()
    {
        StartCoroutine(FromRegisterToLoginCoroutine());
    }

    public void FromLoginToMainMenu()
    {
        StartCoroutine(FromLoginToMainMenuCoroutine());
    }

    private IEnumerator FromLoginToRegisterCoroutine()
    {
        yield return StartCoroutine(FadeOut(loginCanvasGroup));
        loginUsernameInput.text = "";
        loginPasswordInput.text = "";
        loginForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        registerForm.SetActive(true);
        yield return StartCoroutine(FadeIn(registerCanvasGroup));
    }

    private IEnumerator FromRegisterToLoginCoroutine()
    {
        yield return StartCoroutine(FadeOut(registerCanvasGroup));
        usernameInput.text = "";
        passwordInput.text = "";
        confirmPass.text = "";
        email.text = "";
        registerForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        loginForm.SetActive(true);
        yield return StartCoroutine(FadeIn(loginCanvasGroup));
    }

    private IEnumerator FromLoginToMainMenuCoroutine()
    {
        yield return StartCoroutine(FadeOut(loginCanvasGroup));
        loginForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        menu.GetComponent<MainMenu>().enabled = true;
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
