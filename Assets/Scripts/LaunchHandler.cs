using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LaunchHandler : MonoBehaviour
{
    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;

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
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private IEnumerator HandlePersistentLogin()
    {
        // Wait a few seconds to let login data load properly
        yield return new WaitForSeconds(loadTime);

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
}
