using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using System;

public class FirebaseHandler : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    private static DatabaseReference dbReference;
    private static Dictionary<string, string> cookiesDictionary;

    private void Awake()
    {
        cookiesDictionary = new Dictionary<string, string>();

        // Check that all the necessary dependencies for Firebase are on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public static void InitializeFirebase()
    {
        if (auth != null || dbReference != null)
        {
            Debug.LogWarning("Attempt to reinitialize Firebase was aborted.");
            return;
        }

        Debug.Log("Setting up Firebase");
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public static FirebaseAuth GetFirebaseAuth()
    {
        return auth;
    }

    public static DatabaseReference GetDatabaseReference()
    {
        return dbReference;
    }

    public static async Task<Dictionary<string, string>> GetCookiesDictionary(DatabaseReference dbReference, string uid)
    {
        cookiesDictionary = new Dictionary<string, string>();

        string path = $"users/{uid}/cookies";
        DatabaseReference cookiesRef = dbReference.Child(path);

        try
        {
            // Read the data asynchronously
            DataSnapshot snapshot = await cookiesRef.GetValueAsync();

            if (snapshot.HasChildren)
            {
                foreach (var cookieSnapshot in snapshot.Children)
                {
                    cookiesDictionary.Add(cookieSnapshot.Key, cookieSnapshot.Value.ToString());
                }

                return cookiesDictionary;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching cookies: {ex}");
            return null;
        }
    }
}
