using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LoginHandler : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    private Dictionary<string, string> userAccounts;

    private void Start()
    {
        userAccounts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "admin", "password" }
        };
    }

    public void LoginPressed()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == null || password == null || username.Length <= 0 || password.Length <= 0)
        {
            Debug.Log("Error: Username and Password fields must not be left blank.");
            return;
        }

        if (userAccounts.TryGetValue(username, out string correspondingValue))
        {
            Debug.Log("Account found!");

            if (password == correspondingValue)
            {
                Debug.Log("Password matches!");
            }
            else
            {
                Debug.Log("Error: Password does not match!");
                return;
            }
        }
        else
        {
            Debug.Log("Error: Account not found.");
            return;
        }
    }

    public void PresentRegisterPage()
    {
        Debug.Log("Not finished yet...");
    }
}
