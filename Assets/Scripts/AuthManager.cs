using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using Newtonsoft.Json.Linq;
using Firebase.Extensions;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference rootReference;

    // Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    // Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    // UI variables
    [Header("UI")]
    public GameObject registerUI;
    public GameObject loginUI;

    // API variables
    [Header("API")]
    public string ipApiUrl = "http://ip-api.com/json";

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

    public void LoginButtonPressed()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButtonPressed()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        // Call the FirebaseAuth sign in function passing the email and password
        var loginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);

        // Wait until the task completes
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            // If there are errors handle them
            Debug.LogWarning($"Failed to register task with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing password";
                    break;
                case AuthError.WrongPassword:
                    message = "Incorrect password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            user = loginTask.Result.User;

            // Check if the user has verified their email yet
            if (user.IsEmailVerified == false)
            {
                warningLoginText.text = "Please verify your email before signing in";
                confirmLoginText.text = "";
                auth.SignOut();
            }
            else
            {
                // User is now logged in, get the result
                Debug.Log($"User signed in successfully: {user.DisplayName}, {user.Email}");
                warningLoginText.text = "";
                confirmLoginText.text = $"Signed in as {user.DisplayName} ({user.Email})!";

                DateTime currentTime = DateTime.UtcNow;
                DateTimeOffset currentDateTimeOffset = new DateTimeOffset(currentTime);
                long unixTimestampMillis = currentDateTimeOffset.ToUnixTimeMilliseconds();
                rootReference.Child("users").Child(user.UserId).Child("lastSignIn").SetValueAsync(unixTimestampMillis).ContinueWithOnMainThread(task =>
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

                SceneManager.LoadSceneAsync("HomeScene");
            }
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            // If the username is blank, show a warning
            warningRegisterText.text = "Please input your username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            // If the password does not match, show a warning
            warningRegisterText.text = "Passwords do not match";
        }
        else
        {
            // Call the FirebaseAuth register function passing in the email and password
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            // Wait until the task completes
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                // If there are errors handle them
                Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Password is too weak, please try again";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email is already in use, please try again";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                // User has been created, get the result
                user = registerTask.Result.User;

                if (user != null)
                {
                    // Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // Call the FirebaseAuth update user profile function passing the profile with the username
                    var profileTask = user.UpdateUserProfileAsync(profile);

                    // Wait until the task completes
                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                    if (profileTask.Exception != null)
                    {
                        // If there are errors handle them
                        Debug.LogWarning($"Failed to register task with {profileTask.Exception}");
                        FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username set failed!";
                    }
                    else
                    {
                        DateTime currentTime = DateTime.UtcNow;
                        DateTimeOffset currentDateTimeOffset = new DateTimeOffset(currentTime);
                        long unixTimestampMillis = currentDateTimeOffset.ToUnixTimeMilliseconds();

                        string countryCode = "N/A";
                        string countryName = "N/A";

                        string url = ipApiUrl;
                        using (UnityWebRequest www = UnityWebRequest.Get(url))
                        {
                            yield return www.SendWebRequest();

                            if (www.result == UnityWebRequest.Result.Success)
                            {
                                JObject jsonResponse = JObject.Parse(www.downloadHandler.text);
                                countryCode = (string)jsonResponse["countryCode"];
                                countryName = (string)jsonResponse["country"];
                            } else
                            {
                                Debug.LogError("Error fetching GeoIP data: " + www.error);
                            }
                        }

                        Dictionary<string, object> userData = new Dictionary<string, object>
                        {
                            { "userId", user.UserId },
                            { "displayName", user.DisplayName },
                            { "handle", user.DisplayName },
                            { "profilePicture", "undefined" },
                            { "title", "Beginner" },
                            { "lastSignIn",  unixTimestampMillis},
                            { "storiesRead", 0 },
                            { "gamesPlayed", 0 },
                            { "storyXp", 0.0f },
                            { "gameXp", 0.0f },
                            { "storyTitlesWon", 0 },
                            { "gameTitlesWon", 0 },
                            { "countryCode", countryCode },
                            { "countryName", countryName }
                        };
                        rootReference.Child("users").Child(user.UserId).SetValueAsync(userData).ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompleted)
                            {
                                // Username is now set, return to login screen
                                registerUI.SetActive(false);
                                loginUI.SetActive(true);

                                emailLoginField.text = "";
                                passwordLoginField.text = "";
                                warningLoginText.text = "";
                                confirmLoginText.text = "Account has been created, please verify your email before signing in!";

                                usernameRegisterField.text = "";
                                emailRegisterField.text = "";
                                passwordRegisterField.text = "";
                                passwordRegisterVerifyField.text = "";
                                warningRegisterText.text = "";

                                // Send a confirmation email
                                user.SendEmailVerificationAsync().ContinueWith(task =>
                                {
                                    if (task.IsCanceled)
                                    {
                                        Debug.LogError("SendEmailVerificationAsync was canceled.");
                                        return;
                                    }
                                    if (task.IsFaulted)
                                    {
                                        Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                                        return;
                                    }

                                    Debug.Log("Email sent successfully.");

                                    auth.SignOut();
                                });
                            } else
                            {
                                warningRegisterText.text = "Error creating account, please try again";
                                user.DeleteAsync().ContinueWithOnMainThread(task =>
                                {
                                    if (task.IsCompleted)
                                    {
                                        Debug.Log("User account successfully deleted.");
                                    } else
                                    {
                                        Debug.LogError("Error deleting user account: " + task.Exception);
                                    }
                                });
                            }
                        });
                    }
                }
            }
        }
    }

    public void SwitchToLoginPanel()
    {
        registerUI.SetActive(false);
        loginUI.SetActive(true);
    }

    public void SwitchToRegisterPanel()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }
}
