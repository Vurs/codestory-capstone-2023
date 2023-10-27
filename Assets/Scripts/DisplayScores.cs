using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScores : MonoBehaviour
{
    public GameObject homePage;
    public GameObject bottomBar;
    public GameObject endGameStoryPage;
    public TMP_Text elapsedTimeText;
    public TMP_Text xpText;
    public TMP_Text youCompletedActivityText;
    public Image image;
    public GameObject streakPanel;
    public Animator dailyStreakAnimator;
    public TMP_Text oldStreakText;
    public TMP_Text newStreakText;
    public TMP_Text congratsText;
    public CheckDailyStreak dailyStreakChecker;

    // Start is called before the first frame update
    void Start()
    {
        GameObject endActivityHandler = GameObject.Find("EndActivityHandler");
        if (endActivityHandler != null)
        {
            EndActivityHandler statsHolder = endActivityHandler.GetComponent<EndActivityHandler>();
            elapsedTimeText.text = Utils.ConvertToMS(statsHolder.activityElapsedTime);
            xpText.text = statsHolder.activityXp.ToString();
            youCompletedActivityText.text = "You completed " + statsHolder.activityName + " with flying colors!";

            Texture2D texture = Resources.Load<Texture2D>($"{statsHolder.activityCodeName}_Art");
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                image.sprite = sprite;
                image.color = Color.white;
            }

            homePage.SetActive(false);
            bottomBar.SetActive(false);
            endGameStoryPage.SetActive(true);

            Destroy(endActivityHandler);

            DailyStreakHandler.IncrementStreakIfApplicable(streakPanel, dailyStreakAnimator, oldStreakText, newStreakText, congratsText, dailyStreakChecker);
        }
    }
}
