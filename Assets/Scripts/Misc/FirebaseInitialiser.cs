using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Storage;

public class FirebaseInitialiser : MonoBehaviour
{
    public static FirebaseFirestore Database { get; private set; }
    public static FirebaseAuth Auth { get; private set; }
    public static CollectionReference CollectionReference {  get; private set; }
    public static FirebaseStorage Storage { get; private set; }

    public static StorageReference StorageReference { get; private set; }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Database = FirebaseFirestore.DefaultInstance;
                CollectionReference = Database.Collection("usernames");
                Auth = FirebaseAuth.DefaultInstance;
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                Storage = Storage = FirebaseStorage.GetInstance("gs://arofficedatabase.firebasestorage.app");
                StorageReference = Storage.RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: {0} " + dependencyStatus);
            }
        });
    }
}
