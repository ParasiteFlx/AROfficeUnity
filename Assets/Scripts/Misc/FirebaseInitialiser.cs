using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class FirebaseInitialiser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }
}
