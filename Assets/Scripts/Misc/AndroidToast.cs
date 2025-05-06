using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidToast : MonoBehaviour
{
    /// <summary>
    /// https://www.youtube.com/watch?v=6FWtlV7Tur8
    /// </summary>

    private AndroidJavaObject currentActivity;
    public delegate void SendToastDelegate(string message);
    public static SendToastDelegate sendToast;

    // Start is called before the first frame update
    void Start()
    {
        sendToast = SendToast;  
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }

    private void SendToast(string message)
    {
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }
}
