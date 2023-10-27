using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DailyStreakHandler : MonoBehaviour
{
    static string dateFormat = "yyyy-MM-ddTHH:mm:ss";

    public static void HandleStreakOnLoad()
    {
        string currentDateString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        string fallbackDateString = new DateTime(1, 1, 1).ToString("yyyy-MM-ddTHH:mm:ss");

        DatabaseHandler.FetchDatabaseValue("users/{0}/lastActivityCompleted", fallbackDateString, (dateString) =>
        {
            DateTime lastActivityCompletedDateTime = ConvertStringToDateTime(dateString);
            DateTime currentDateTime = ConvertStringToDateTime(currentDateString);

            if (lastActivityCompletedDateTime.Date < currentDateTime.Date.AddDays(-1))
            {
                // They missed a day, set their streak to 0
                DatabaseHandler.SetDatabaseValue("users/{0}/dailyStreak", 0);
                Debug.Log("Resetting streak to 0");
            } else
            {
                Debug.Log("Streak is still intact, leaving it as is");
            }
        });
    }

    public static void IncrementStreakIfApplicable(GameObject streakPanel, Animator animator, TMP_Text oldStreakText, TMP_Text newStreakText, TMP_Text congratsText, CheckDailyStreak dailyStreakChecker)
    {
        string currentDateString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        string fallbackDateString = new DateTime(1, 1, 1).ToString("yyyy-MM-ddTHH:mm:ss");

        DatabaseHandler.FetchDatabaseValue("users/{0}/lastActivityCompleted", fallbackDateString, (dateString) =>
        {
            DateTime lastActivityCompletedDateTime = ConvertStringToDateTime(dateString);
            DateTime currentDateTime = ConvertStringToDateTime(currentDateString);

            if (lastActivityCompletedDateTime.Date == currentDateTime.Date.AddDays(-1))
            {
                // Their last activity was completed the day before, increment their streak
                DatabaseHandler.FetchDatabaseValue("users/{0}/dailyStreak", 0, (value) =>
                {
                    DatabaseHandler.SetDatabaseValue("users/{0}/dailyStreak", value + 1);
                    DatabaseHandler.SetDatabaseValue("users/{0}/lastActivityCompleted", currentDateString);
                    Debug.Log("Incrementing dailyStreak by 1 and setting lastActivityCompleted to today");

                    oldStreakText.text = value.ToString();
                    newStreakText.text = (value + 1).ToString();
                    congratsText.text = "Congrats on keeping your streak alive!";
                    streakPanel.SetActive(true);
                    animator.SetBool("DailyStreakActivated", true);
                    dailyStreakChecker.PlaySuccessSound();
                });
            }
            else if (lastActivityCompletedDateTime.Date < currentDateTime.Date.AddDays(-1))
            {
                // They missed a day, set their streak to 0
                DatabaseHandler.SetDatabaseValue("users/{0}/dailyStreak", 1);
                DatabaseHandler.SetDatabaseValue("users/{0}/lastActivityCompleted", currentDateString);
                Debug.Log("Last activity was completed over a day ago, setting dailyStreak to 1");

                oldStreakText.text = "0";
                newStreakText.text = "1";
                congratsText.text = "Congrats on starting a new streak! Keep it up!";
                streakPanel.SetActive(true);
                animator.SetBool("DailyStreakActivated", true);
                dailyStreakChecker.PlaySuccessSound();
            } else
            {
                Debug.Log("Last activity was completed today, leaving dailyStreak as is");
            }
        });
    }

    static DateTime ConvertStringToDateTime(string dateTimeString)
    {
        DateTime outputtedDateTime;
        if (DateTime.TryParseExact(dateTimeString, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputtedDateTime))
        {
            return outputtedDateTime;
        }
        else
        {
            Debug.LogError("Could not convert string to DateTime object");
            return new DateTime(1, 1, 1);
        }
    }
}
