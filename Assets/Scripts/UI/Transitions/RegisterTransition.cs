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
        StartCoroutine(Transitions.Instance().CanvasFadeIn(loginCanvasGroup));
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
        yield return StartCoroutine(Transitions.Instance().CanvasFadeOut(loginCanvasGroup));
        loginUsernameInput.text = "";
        loginPasswordInput.text = "";
        loginForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        registerForm.SetActive(true);
        yield return StartCoroutine(Transitions.Instance().CanvasFadeIn(registerCanvasGroup));
    }

    private IEnumerator FromRegisterToLoginCoroutine()
    {
        yield return StartCoroutine(Transitions.Instance().CanvasFadeOut(registerCanvasGroup));
        usernameInput.text = "";
        passwordInput.text = "";
        confirmPass.text = "";
        email.text = "";
        registerForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        loginForm.SetActive(true);
        yield return StartCoroutine(Transitions.Instance().CanvasFadeIn(loginCanvasGroup));
    }

    private IEnumerator FromLoginToMainMenuCoroutine()
    {
        yield return StartCoroutine(Transitions.Instance().CanvasFadeOut(loginCanvasGroup));
        loginForm.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        menu.GetComponent<MainMenu>().enabled = true;
    }
   

}
