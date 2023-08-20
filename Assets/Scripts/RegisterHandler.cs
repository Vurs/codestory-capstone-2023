using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterHandler : MonoBehaviour
{
    public void PresentLoginPage()
    {
        SceneManager.LoadSceneAsync("LoginScene");
    }
}
