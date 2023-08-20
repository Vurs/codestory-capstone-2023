using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    public void PresentRegisterPage()
    {
        SceneManager.LoadSceneAsync("RegisterScene");
    }
}
