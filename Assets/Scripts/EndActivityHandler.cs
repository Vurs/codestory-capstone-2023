using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class EndActivityHandler : MonoBehaviour
{
    public int activityXp;
    public int activityElapsedTime;
    public string activityType;
    public string activityName;
    public string activityCodeName;

    public void EndActivity(GameObject obj, int xp, int elapsedTime)
    {
        if (activityType == "MINIGAME")
        {
            DatabaseHandler.IncrementStat("gameXp", xp);
            DatabaseHandler.IncrementStat("gamesPlayed", 1);
        } else
        {
            DatabaseHandler.IncrementStat("storyXp", xp);
            DatabaseHandler.IncrementStat("storiesRead", 1);
        }

        activityXp = xp;
        activityElapsedTime = elapsedTime;

        DontDestroyOnLoad(obj);

        SceneManager.LoadSceneAsync("HomeScene");
    }
}
