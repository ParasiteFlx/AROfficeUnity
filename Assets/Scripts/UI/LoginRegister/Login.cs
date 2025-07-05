using Firebase.Auth;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField username, password;
    [SerializeField]
    private TextMeshProUGUI usernameErrorField, passwordErrorField, userID;

    private bool EmptyFieldCheck(bool forgotPassword)
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

        if(!forgotPassword)
        {
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
        }
     
        return emptyFields;
    }

    private async Task<DocumentSnapshot> UsernameExistsAsync(string username)
    {
        //The snapshot shows the database at the moment when the reading operations is finished. 
        DocumentSnapshot snapshot = await FirebaseInitialiser.CollectionReference.Document(username).GetSnapshotAsync();
        return snapshot;
    }

    public async void LoginUserAsync()
    {
      
        bool emptyFields = EmptyFieldCheck(false);

        if (!emptyFields) {
            
            DocumentSnapshot snapshot = await UsernameExistsAsync(username.text);
            if(snapshot.Exists)
            {
                Dictionary<string, object> emailField = snapshot.ToDictionary();
                string email = emailField["Email"].ToString();
                try
                {
                    Firebase.Auth.AuthResult authResult = await FirebaseInitialiser.Auth.SignInWithEmailAndPasswordAsync(email, password.text);
                    RegisterTransition.fromLogToMainMenu();
                    userID.text = authResult.User.UserId.Substring(0,7);
                }
                catch (Firebase.FirebaseException authException)
                {
                    AndroidToast.sendToast("Wrong username or password!");
                }                             
            }
            else
            {
                AndroidToast.sendToast("Username does not exist!");
            }     
        }
     
    }

    public async void ForgotPassword()
    {
        bool emptyFields = EmptyFieldCheck(true);
        if (!emptyFields)
        {
            DocumentSnapshot snapshot = await UsernameExistsAsync(username.text);
            if (snapshot.Exists)
            {
                FirebaseInitialiser.Auth.SendPasswordResetEmailAsync(snapshot.GetValue<string>("Email"));
                AndroidToast.sendToast("Password reset email sent!");
            }
            else
            {
                AndroidToast.sendToast("Username does not exist!");
            }
        }
      
    }

}
