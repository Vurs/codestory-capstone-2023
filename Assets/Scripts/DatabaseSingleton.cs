using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using System.Threading.Tasks;
using TMPro;

public class DatabaseSingleton : Singleton<DatabaseSingleton>
{
    private DatabaseReference dbReference;

    public async void CreateUser()
    {
        TMP_InputField emailInput = GameObject.Find("EmailInput").GetComponent<TMP_InputField>();
        TMP_InputField passwordInput = GameObject.Find("PasswordInput").GetComponent<TMP_InputField>();

        string emailInputText = emailInput.text;
        string passwordInputText = passwordInput.text;

        // Input validation would be performed here before proceeding
        if (emailInputText == null || passwordInputText == null || emailInputText.Length <= 0 || passwordInputText.Length <= 0)
        {
            Debug.Log("Error: Username and Password fields must not be left blank.");
            return;
        }

        Query query = dbReference.Child("users").OrderByChild("email").EqualTo(emailInputText);
        DataSnapshot snapshot = await query.GetValueAsync().ConfigureAwait(false);

        if (snapshot.Exists)
        {
            Debug.Log("Email already exists. Please enter a different email.");
            return;
        }

        string userId = dbReference.Child("users").Push().Key;

        // For testing purposes, will remove this later
        string testUsername = $"user-{userId}";

        // User newUser = new(testUsername, emailInputText, passwordInputText);
        // string json = JsonUtility.ToJson(newUser);
        // await dbReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ConfigureAwait(false);

        Debug.Log($"User {testUsername} added successfully!");
    }

    // public async Task<User> GetUserById(int userId)
    // {
    //     DataSnapshot snapshot = await dbReference.Child("users").Child(userId.ToString()).GetValueAsync().ConfigureAwait(false);

    //     string json = snapshot.GetRawJsonValue();
    //     User user = JsonUtility.FromJson<User>(json);

    //     return user;
    // }

    public void InitializeFirebase()
    {
        DontDestroyOnLoad(gameObject);

        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Initialized Firebase");
        });
    }

    public bool IsFirebaseInitialized()
    {
        return dbReference != null;
    }
}
