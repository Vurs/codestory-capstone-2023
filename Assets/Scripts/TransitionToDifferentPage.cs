using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToDifferentPage : MonoBehaviour
{
    public GameObject oldPanel;
    public GameObject newPanel;

    public void Segue()
    {
        oldPanel.SetActive(false);
        newPanel.SetActive(true);
    }

    public void LoadGameStoryView()
    {
        // In the future we can use the game's ID to fetch its information from the Firebase DB and populate the text labels accordingly
        Segue();
    }
}
