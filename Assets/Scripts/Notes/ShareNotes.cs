using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ShareNotes : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI userID;

    public async void UploadSave()
    {
     
        string notesPath = Application.persistentDataPath + "/Notes.json";
        byte[] fileBytes = null;
        fileBytes = File.ReadAllBytes(notesPath);
        // Debug.Log("asta e path la notes sa stii : " + notesPath);
        StorageReference userFolderRef = FirebaseInitialiser.StorageReference.Child("Users").Child(userID.text);

        StorageReference fileRef = userFolderRef.Child("Notes.json");

        try
        {
            StorageMetadata metadata = await fileRef.PutBytesAsync(fileBytes);
            AndroidToast.sendToast("Save uploaded successfully!");
        }
        catch(Firebase.Storage.StorageException storageEx)
        {
           // Debug.Log("De asta nu e uploadata : " + storageEx.ErrorCode);
            AndroidToast.sendToast("An error occured while uploading the save!");
        }

    }

    private async Task<bool> FileExistsAsync(string userID)
    {
        string filePath = "users/" + userID;
      
        StorageReference fileRefference = FirebaseInitialiser.StorageReference.Child(filePath);
        try
        {
            StorageMetadata fileMetadata = await fileRefference.GetMetadataAsync();
            return true;
        }
        catch(Firebase.Storage.StorageException exception)
        {
            if(exception.ErrorCode == StorageException.ErrorObjectNotFound)
            {

                AndroidToast.sendToast("The save or the user does not exist!");
                
            }
            return false;
        }
    }
}
