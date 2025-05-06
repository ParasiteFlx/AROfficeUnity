using TMPro;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Firestore;
using System.Collections.Generic;

public class Register : MonoBehaviour
{   
    [SerializeField]
    private TMP_InputField username, password, confirmPass, email;
    [SerializeField]
    GameObject usernameErrorField, passwordErrorField, confirmPassErrorField, emailErrorField;
    bool error, usernameError, passwordError, confirmPassError, emailError;
    public delegate void OnRegister(bool hasErrors);
    public static OnRegister usernameCheck, passwordCheck, confirmPassCheck, emailCheck;

    private void Start()
    {    
        usernameError = false;
        passwordError = false;
        confirmPassError = false;
        emailError = false;
        usernameCheck = UsernameCheck;
        passwordCheck = PasswordCheck;
        confirmPassCheck = ConfirmPassCheck;
        emailCheck = EmailCheck;
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

    private bool EmptyFieldCheck()
    {
        bool emptyFields = false;
        if (username.text.Length > 0)
        {
            if (usernameErrorField.GetComponent<TextMeshProUGUI>().text.Equals("Field is empty!"))
            {
                usernameErrorField.GetComponent<TextMeshProUGUI>().text = "";
            }       
        }
        else
        {
            usernameErrorField.GetComponent<TextMeshProUGUI>().text = "Field is empty!";
            emptyFields = true; 
        }

        if (password.text.Length > 0)
        {
           
            if (passwordErrorField.GetComponent<TextMeshProUGUI>().text.Equals("Field is empty!"))
            {
               
                passwordErrorField.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        else
        {
                  
            passwordErrorField.GetComponent<TextMeshProUGUI>().text = "Field is empty!";
            emptyFields = true;
        }

        if (confirmPass.text.Length > 0)
        {
        
            if (confirmPassErrorField.GetComponent<TextMeshProUGUI>().text.Equals("Field is empty!"))
            {
             
                confirmPassErrorField.GetComponent<TextMeshProUGUI>().text = "";
            }        
        }
        else
        {
          
            confirmPassErrorField.GetComponent<TextMeshProUGUI>().text = "Field is empty!";
            emptyFields = true;
        }

        if (email.text.Length > 0)
        {  
            if(emailErrorField.GetComponent<TextMeshProUGUI>().text.Equals("Field is empty!"))
            {
                emailErrorField.GetComponent<TextMeshProUGUI>().text = "";
            }           
        }
        else
        {
            emailErrorField.GetComponent<TextMeshProUGUI>().text = "Field is empty!";
            emptyFields = true;
        }

        return emptyFields;
    }

    private void ErrorCheck()
    {
        bool emptyfields = EmptyFieldCheck();

        if (!usernameError && !passwordError && !confirmPassError && !emailError && !emptyfields )
        {
            error = false;
        }
        else
        {
            error = true;
        }
    }

    private async Task<bool> UsernameExistsAsync(string username)
    {
        //The snapshot shows the database at the moment when the reading operations is finished. 
        DocumentSnapshot snapshot = await FirebaseInitialiser.CollectionReference.Document(username).GetSnapshotAsync();
        return snapshot.Exists;
    }

    private async Task<bool> EmailExistsAsync(string email)
    {         
        Firebase.Firestore.Query query = FirebaseInitialiser.CollectionReference.WhereEqualTo("Email", email);
        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
        if (querySnapshot.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async void RegisterUserAsync()
    {
        if(await ExistingCredentialsCheckAsync())
        {          
            await FirebaseInitialiser.Auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text);
            Dictionary<string, object> userData = new Dictionary<string, object>
                    {
                        { "Email", email.text }
                    };
            DocumentReference userRef = FirebaseInitialiser.CollectionReference.Document(username.text);
            await userRef.SetAsync(userData);
            RegisterTransition.fromLogToReg();
            AndroidToast.sendToast("User account has been created!");
        }
    }
    
    private async Task<bool> ExistingCredentialsCheckAsync()
    {
   
        ErrorCheck();
        
        if (!error)
        {       
            bool uniqueUsername = false;
            bool uniqueEmail = false;          
            bool usernameExists = await UsernameExistsAsync(username.text);
            bool emailExists = await EmailExistsAsync(email.text);
           // Debug.Log("Username exists:" + usernameExists + "Email exists: " + emailExists);
            if (usernameExists)
            {               
                usernameErrorField.GetComponent<TextMeshProUGUI>().text = "Username is already taken!";
            }
            else
            {
                uniqueUsername = true;
                usernameErrorField.GetComponent<TextMeshProUGUI>().text = "";

            }
            if (emailExists)
            {
                
                emailErrorField.GetComponent<TextMeshProUGUI>().text = "There already is an account with this email!";
            }
            else
            {
                uniqueEmail = true;
                emailErrorField.GetComponent<TextMeshProUGUI>().text = "";
            }
            if (uniqueUsername && uniqueEmail)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
