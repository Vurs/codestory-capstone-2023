using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplayScores : MonoBehaviour
{
    public GameObject homePage;
    public GameObject bottomBar;
    public GameObject endGameStoryPage;
    public TMP_Text elapsedTimeText;
    public TMP_Text xpText;
    public GameObject streakPanel;
    public Animator dailyStreakAnimator;
    public TMP_Text oldStreakText;
    public TMP_Text newStreakText;

    // Start is called before the first frame update
    void Start()
    {
        GameObject endActivityHandler = GameObject.Find("EndActivityHandler");
        if (endActivityHandler != null )
        {
            EndActivityHandler statsHolder = endActivityHandler.GetComponent<EndActivityHandler>();
            elapsedTimeText.text = Utils.ConvertToMS(statsHolder.activityElapsedTime);
            xpText.text = statsHolder.activityXp.ToString();

            homePage.SetActive(false);
            bottomBar.SetActive(false);
            endGameStoryPage.SetActive(true);

            Destroy(endActivityHandler);

            DailyStreakHandler.IncrementStreakIfApplicable(streakPanel, dailyStreakAnimator, oldStreakText, newStreakText);
        }
    }
}
