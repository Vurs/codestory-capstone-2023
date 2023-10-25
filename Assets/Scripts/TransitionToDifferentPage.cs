using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionToDifferentPage : MonoBehaviour
{
    public GameObject oldPanel;
    public GameObject newPanel;
    public GameObject bottomBar;
    public GameObject newSelectedTab;
    public GameObject[] allTabs;
    public bool isStreakPanel = false;
    public Animator streakAnimator;
    public ProfilePopulator populator;

    public void Segue()
    {
        oldPanel.SetActive(false);
        newPanel.SetActive(true);

        if (bottomBar != null)
        {
            bottomBar.SetActive(true);
        }

        if (isStreakPanel == true)
        {
            if (streakAnimator != null)
            {
                streakAnimator.SetBool("DailyStreakActivated", false);
                populator.RepopulateHome();
            }
        }
    }

    public void ChangeTabColors()
    {
        foreach (GameObject tab in allTabs)
        {
            Image tabImage = tab.GetComponent<Image>();
            tabImage.color = Utils.TabColors["GRAY"];
        }

        Image newSelectedTabImage = newSelectedTab.GetComponent<Image>();
        newSelectedTabImage.color = Utils.TabColors["BLUE"];
    }
}
