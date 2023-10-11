using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class EndActivityHandler : MonoBehaviour
{
    public int activityXp;
    public int activityElapsedTime;

    public void EndActivity(GameObject obj, int xp, int elapsedTime)
    {
        DatabaseHandler.IncrementStat("gameXp", xp);
        DatabaseHandler.IncrementStat("gamesPlayed", 1);

        activityXp = xp;
        activityElapsedTime = elapsedTime;

        DontDestroyOnLoad(obj);

        SceneManager.LoadSceneAsync("HomeScene");
    }
}
