using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOutHandler : MonoBehaviour
{
    public void SignOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();

        SceneManager.LoadSceneAsync("AuthenticationScene");
    }
}
