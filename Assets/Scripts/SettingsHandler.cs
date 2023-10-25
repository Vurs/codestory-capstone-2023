using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField emailInput;
    public Image profilePictureImage;

    private int currentPictureIndex = 1;
    private ProfilePictureManager profilePictureManager;
    private ProfilePopulator profilePopulator;

    void Start()
    {
        profilePictureManager = GameObject.Find("ProfilePictureManager").GetComponent<ProfilePictureManager>();
        profilePopulator = GameObject.Find("ProfilePopulator").GetComponent<ProfilePopulator>();
        DatabaseHandler.FetchDatabaseValue("users/{0}/profilePicture", 1, (value) =>
        {
            currentPictureIndex = value;
            profilePictureManager.SetImage(profilePictureImage, profilePictureManager.profilePictures["pfp_" + value.ToString("D3")]);
        });
    }

    public void ChangeEmail()
    {
        if (string.IsNullOrEmpty(emailInput.text)) return;

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string newEmail = emailInput.text;
            user.UpdateEmailAsync(newEmail).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("User email updated successfully");
                } else
                {
                    Debug.LogError("Could not complete user email update operation: " + task.Exception);
                }
            });
        } else
        {
            Debug.LogError("No user is currently signed in");
        }
    }

    public void ChangePassword()
    {
        if (string.IsNullOrEmpty(passwordInput.text)) return;

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string newPassword = passwordInput.text;
            user.UpdatePasswordAsync(newPassword).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("User password updated successfully");
                } else
                {
                    Debug.LogError("Could not complete user password update operation: " + task.Exception);
                }
            });
        } else
        {
            Debug.LogError("No user is currently signed in");
        }
    }
    
    public void ChangeName()
    {
        if (string.IsNullOrEmpty(nameInput.text)) return;

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string newName = nameInput.text;
            DatabaseHandler.SetDatabaseValue("users/{0}/displayName", newName);
        }
    }

    public void IncrementProfilePicture()
    {
        if (currentPictureIndex + 1 > 6)
        {
            currentPictureIndex = 1;
        } else
        {
            currentPictureIndex++;
        }

        profilePictureManager.SetImage(profilePictureImage, profilePictureManager.profilePictures["pfp_" + currentPictureIndex.ToString("D3")]);
        DatabaseHandler.SetDatabaseValue("users/{0}/profilePicture", currentPictureIndex);
    }

    public void OnProfileButtonPressed()
    {
        ChangeName();
        ChangePassword();
        ChangeEmail();

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            profilePopulator.RepopulateHome();
            Debug.Log("Saved settings successfully");
        }
    }

    public void OnChangeProfilePicturePressed()
    {
        IncrementProfilePicture();
    }
}
