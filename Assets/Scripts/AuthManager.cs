using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using System.Threading.Tasks;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference dbReference;

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

    private void Awake()
    {
        auth = FirebaseHandler.GetFirebaseAuth();
        if (auth == null)
        {
            Debug.LogError("FirebaseAuth is null, the app will not perform correctly.");
        }

        dbReference = FirebaseHandler.GetDatabaseReference();
        if (dbReference == null)
        {
            Debug.LogError("FirebaseDatabase is null, the app will not perform correctly.");
        }
    }

    public void LoginButtonPressed()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButtonPressed()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator LoginAsync(string _email, string _password)
    {
        yield return Login(emailLoginField.text, passwordLoginField.text);
    }

    private async Task Login(string _email, string _password)
    {
        // Call the FirebaseAuth sign in function passing the email and password
        try
        {
            var loginTask = await auth.SignInWithEmailAndPasswordAsync(_email, _password);
            user = loginTask.User;

            // Check if the user has verified their email yet
            if (user.IsEmailVerified == false)
            {
                warningLoginText.text = "Please verify your email before signing in";
                confirmLoginText.text = "";
            }
            else
            {
                // User is now logged in, get the result
                Debug.Log($"User signed in successfully: {user.DisplayName}, {user.Email}");
                warningLoginText.text = "";
                confirmLoginText.text = "Signed in!";

                // Create a cookie key and value
                string deviceIdentifier = SystemInfo.deviceUniqueIdentifier;
                if (deviceIdentifier != SystemInfo.unsupportedIdentifier && deviceIdentifier != "")
                {
                    Dictionary<string, string> existingCookies = await FirebaseHandler.GetCookiesDictionary(dbReference, user.UserId);
                    if (existingCookies == null)
                    {
                        Debug.LogError("There was an error while trying to fetch the user's existing cookies.");
                    }
                    else
                    {
                        string cookie = dbReference.Push().Key;

                        // Set the UID and Cookie PlayerPrefs for autologin purposes
                        PlayerPrefs.SetString("UID", user.UserId);
                        PlayerPrefs.SetString("Cookie", cookie);

                        existingCookies[deviceIdentifier] = cookie;

                        // Create the cookie in the user's database entry
                        string userPath = $"users/{user.UserId}/cookies";

                        await dbReference.Child(userPath).SetValueAsync(existingCookies);
                        Debug.Log("User entry created successfully in the 'users' table.");
                    }
                }
                else
                {
                    Debug.LogWarning("Device identifier is unsupported, could not create an autologin cookie!");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Debug.LogWarning($"Failed to register task with {ex}");

            if (typeof(FirebaseException) == ex.InnerException.GetType())
            {
                FirebaseException firebaseException = (FirebaseException)ex.InnerException;
                AuthError errorCode = (AuthError)firebaseException.ErrorCode;

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
                // Handle other exceptions
                Debug.LogError($"Unexpected exception: {ex}");
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
