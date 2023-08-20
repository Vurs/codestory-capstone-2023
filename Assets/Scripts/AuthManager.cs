using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

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
            }
            else
            {
                // User is now logged in, get the result
                Debug.Log($"User signed in successfully: {user.DisplayName}, {user.Email}");
                warningLoginText.text = "";
                confirmLoginText.text = "Signed in!";
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
