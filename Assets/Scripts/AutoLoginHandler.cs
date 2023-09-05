using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class AutoLoginHandler : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(InitializeAsync());
    }

    private IEnumerator InitializeAsync()
    {
        yield return PerformAsyncInitialization();
    }

    private async Task PerformAsyncInitialization()
    {
        // Initialize Firebase
        FirebaseHandler.InitializeFirebase();

        string deviceIdentifier = SystemInfo.deviceUniqueIdentifier;

        if (deviceIdentifier == SystemInfo.unsupportedIdentifier)
        {
            // If the device identifier is unsupported, fallback to the authentication scene
            Debug.LogWarning("The device identifier is unsupported, falling back to authentication scene...");
            SceneManager.LoadSceneAsync("AuthenticationScene");
        }
        else
        {
            // Search the user's Firebase DB for a cookie matching their device identifier
            DatabaseReference dbReference = FirebaseHandler.GetDatabaseReference();
            if (dbReference == null)
            {
                // If the DB reference is null, fallback to the authentication scene
                Debug.LogWarning("The database reference object is null, falling back to authentication scene...");
                SceneManager.LoadSceneAsync("AuthenticationScene");
                return;
            }

            string uid = PlayerPrefs.GetString("UID");
            if (uid == null || uid == "")
            {
                // If the UID PlayerPref is null, fallback to the authentication scene
                Debug.LogWarning("The UID PlayerPref is null, falling back to authentication scene...");
                SceneManager.LoadSceneAsync("AuthenticationScene");
                return;
            }

            Dictionary<string, string> cookies = await FirebaseHandler.GetCookiesDictionary(dbReference, uid);
            if (cookies == null)
            {
                Debug.LogWarning("No autologin cookies found for this device on the remote database, falling back to authentication scene...");
                SceneManager.LoadSceneAsync("AuthenticationScene");
                return;
            }

            bool hasCookie = cookies.ContainsKey(deviceIdentifier);
            if (hasCookie == true)
            {
                // If found, check the user's PlayerPrefs to see if it matches on both ends
                string cookie = PlayerPrefs.GetString("Cookie");
                if (cookie != null && cookie != "" && cookie == cookies[deviceIdentifier])
                {
                    // If matching, skip authentication scene and send user to the home scene
                    Debug.Log("Found autologin cookie, sending user to home scene!");
                }
                else
                {
                    // If not matching, fallback to the authentication scene
                    Debug.LogWarning("The cookie stored on the user's device does not exist or does not match the one stored in the remote database, falling back to authentication scene...");
                    SceneManager.LoadSceneAsync("AuthenticationScene");
                }
            }
            else
            {
                // If not found, fallback to the authentication scene
                Debug.LogWarning("No autologin cookies found for this device on the remote database, falling back to authentication scene...");
                SceneManager.LoadSceneAsync("AuthenticationScene");
            }
        }
    }
}
