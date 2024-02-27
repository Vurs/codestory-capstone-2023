using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using System;
using TMPro;

public class LaunchHandler : MonoBehaviour
{
    public VersionChecker versionChecker;
    public TMP_Text statusText;

    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference rootReference;

    public float loadTime = 5f;

    private void Awake()
    {
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

        StartCoroutine(HandlePersistentLogin());
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        rootReference = FirebaseDatabase.DefaultInstance.RootReference;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        try
        {
            if (auth.CurrentUser != null && user != auth.CurrentUser)
            {
                bool signedIn = user != null && auth.CurrentUser != null;
                if (!signedIn && user != null)
                {
                    Debug.Log("Signed out " + user.UserId);
                }
                user = auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log("Signed in " + user.UserId);

                    DateTime currentTime = DateTime.UtcNow;
                    DateTimeOffset currentDateTimeOffset = new DateTimeOffset(currentTime);
                    long unixTimestampMillis = currentDateTimeOffset.ToUnixTimeMilliseconds();

                    var userReference = rootReference.Child("users").Child(user.UserId);
                    if (userReference != null)
                    {
                        var lastSignInReference = userReference.Child("lastSignIn");
                        if (lastSignInReference != null)
                        {
                            lastSignInReference.SetValueAsync(unixTimestampMillis).ContinueWithOnMainThread(task =>
                            {
                                if (task.IsCompleted)
                                {
                                    Debug.Log("Last sign-in timestamp updated in the database.");
                                }
                                else
                                {
                                    Debug.LogError("Error updating last sign-in timestamp: " + task.Exception);
                                }
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in AuthStateChanged: " + ex.Message);
        }
    }

    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private IEnumerator HandlePersistentLogin()
    {
        float loadSpinnerTime = 0.5f;
        yield return new WaitForSeconds(loadSpinnerTime);

        statusText.text = "Loading assets...";
        AnimateSpinner.isSpinning = true;

        // Wait a few seconds to let login data load properly
        yield return new WaitForSeconds(loadTime - loadSpinnerTime);

        StartCoroutine(versionChecker.CheckForUpdate(statusText, () =>
        {
            if (versionChecker.isLatestVersion == true)
            {
                user = auth.CurrentUser;
                if (user != null)
                {
                    // User has already logged in before, send them to the home page
                    SceneManager.LoadSceneAsync("HomeScene");
                }
                else
                {
                    // User is not already logged in, send them to the authentication page
                    SceneManager.LoadSceneAsync("AuthenticationScene");
                }
            }
        }));
    }
}
