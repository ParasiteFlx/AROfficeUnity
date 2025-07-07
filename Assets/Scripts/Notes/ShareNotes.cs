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
    [SerializeField]
    TMP_InputField userIDToDownload;
    [SerializeField]
    CanvasGroup shareInterface, restartApp;

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

    public async void DownloadNotesAsync()
    {
        string notesPath = Application.persistentDataPath + "/Notes.json";
        byte[] notesFile = await FileExistsAsync(userIDToDownload.text);
        if(notesFile!= null)
        {
            File.WriteAllBytes(notesPath, notesFile);
            AndroidToast.sendToast("Notes downloaded succesfully!");
            StartCoroutine(Transitions.Instance().CanvasFadeOut(shareInterface));
            StartCoroutine(Transitions.Instance().CanvasFadeIn(restartApp));
          
        }
    }

    public async void ResetNotesAsync()
    {
        string notesPath = Application.persistentDataPath + "/Notes.json";
        byte[] notesFile = await FileExistsAsync(userID.text);
        if (notesFile != null)
        {
            File.WriteAllBytes(notesPath, notesFile);
            AndroidToast.sendToast("Notes downloaded succesfully!");
            StartCoroutine(Transitions.Instance().CanvasFadeOut(shareInterface));
            StartCoroutine(Transitions.Instance().CanvasFadeIn(restartApp));

        }
    }

    private async Task<byte[]> FileExistsAsync(string userID)
    {
        long maxAllowedSize = 1 * 1024 * 1024;
        byte[] fileBytes = null;
        StorageReference folderRef = FirebaseInitialiser.StorageReference.Child("Users").Child(userID);
        StorageReference fileRef = folderRef.Child("Notes.json");
        try
        {
            fileBytes = await fileRef.GetBytesAsync(maxAllowedSize);
            if(fileBytes.Length > 0)
            {
                return fileBytes;
            }
            else
            {
                AndroidToast.sendToast("The user has no data available!");
                return null; 
            }
        }
        catch
        {
            AndroidToast.sendToast("The user has no data available or the data is corrupted!");
            return null;
        }
    }
}
