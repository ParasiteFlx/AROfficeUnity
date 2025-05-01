using TMPro;
using UnityEngine;
using Firebase.Storage;
using Firebase.Extensions;

public class Register : MonoBehaviour
{

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Storage.FirebaseStorage storage;

    User User;
    [SerializeField]
    private TMP_InputField username;
    [SerializeField] 
    private TMP_InputField password;
    [SerializeField]    
    private TMP_InputField email;
    bool error, usernameError, passwordError, confirmPassError, emailError;
    public delegate void OnRegister(bool hasErrors);
    public static OnRegister usernameCheck, passwordCheck, confirmPassCheck, emailCheck;

    private void Start()
    {   
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        User = new User();
        usernameError = false;
        passwordError = false;
        confirmPassError = false;
        emailError = false;
        usernameCheck = UsernameCheck;
        passwordCheck = PasswordCheck;
        confirmPassCheck = ConfirmPassCheck;
        emailCheck = EmailCheck;
    }

    private void registerData()
    {
       User.username = username.text; 
       User.password = password.text;
       User.email = email.text;
    }

    private void ErrorCheck()
    {
        if (!usernameError && !passwordError && !confirmPassError && !emailError)
        {
            error = false;
        }
        else
        {
            error = true;
        }
    }

    private void UsernameCheck(bool hasError)
    {
        usernameError = hasError;
    }    

    private void PasswordCheck(bool hasError)
    {
        passwordError = hasError;
    }

    private void ConfirmPassCheck(bool hasError)
    {
       confirmPassError = hasError;
    }

    private void EmailCheck(bool hasError)
    {
        emailError = hasError;
    }

    public void HasErrors()
    {
        ErrorCheck();
        if(!error)
        {
            GameObject usernameErrorField = GameObject.FindGameObjectWithTag("userError");
            GameObject emailErrorField = GameObject.FindGameObjectWithTag("emailError");
        }
    }
}

public class User
{
    public string username;
    public string password;
    public string email;
}
