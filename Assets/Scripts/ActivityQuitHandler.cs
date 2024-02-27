using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivityQuitHandler : MonoBehaviour
{
    public Button xButton;
    public GameObject popUp;
    public TMP_Text popUpTitle;
    public TMP_Text popUpText;
    public Button yesButton;
    public Button noButton;

    public void OnXButtonClicked()
    {
        popUpTitle.text = "Are You Sure?";
        popUpText.text = "Are you sure you'd like to quit and lose your progress?";
        popUp.SetActive(true);
    }

    public void OnYesButtonClicked()
    {
        SceneManager.LoadSceneAsync("HomeScene");
    }

    public void OnNoButtonClicked()
    {
        popUp.SetActive(false);
    }
}
